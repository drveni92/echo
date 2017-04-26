(function() {
    angular
        .module("Billing")
        .controller('AgentsController', ['$scope', '$http', '$uibModal', 'DataFactory', 'AgentsFactory', function($scope, $http, $uibModal, DataFactory, AgentsFactory) {
            $scope.maxPagination = BillingConfig.maxPagination

            function ListAgents() {
                DataFactory.list("agents?page=" + ($scope.currentPage - 1), function(data) {
                    $scope.agents = data.list;
                    $scope.totalItems = data.totalItems;
                    $scope.currentPage = data.currentPage + 1;
                });
            }

            $scope.pageChanged = function() {
                ListAgents();
            };

            ListAgents();

            $scope.new = function() {
                var modalInstance = $uibModal.open({
                    animation: true,
                    focus: false,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/agents/templates/new.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return AgentsFactory.empty();
                        }
                    }
                });
                modalInstance.result.then(function(agent) {
                    DataFactory.insert("agents", AgentsFactory.agent(agent), function(data) { ListAgents(); });
                }, function() {
                });
            };

            $scope.edit = function(agent) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    focus: false,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/agents/templates/edit.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return $.extend(true, {}, agent)
                        }
                    }
                });
                modalInstance.result.then(function(agent) {
                    DataFactory.update("agents", agent.id, AgentsFactory.agent(agent), function(data) {
                        ListAgents();
                    });
                }, function() {
                    ListAgents();
                });
            };

            $scope.delete = function(agent) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/agents/templates/delete.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return agent
                        },
                        options: function() {
                            return []
                        }
                    }
                });

                modalInstance.result.then(function(agent) {
                    DataFactory.delete("agents", agent.id, function(data) {
                        ToasterService.pop('success', "Success", "Agent deleted");
                        ListAgents();
                    });
                }, function() {
                });
            }

        }]);
}());
