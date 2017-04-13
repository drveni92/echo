(function() {
    angular
        .module("Billing")
        .controller('AgentsController', ['$scope', '$http', '$uibModal', 'DataFactory', function($scope, $http, $uibModal, DataFactory) {
            $scope.maxPagination = BillingConfig.maxPagination

            function ListAgents(page) {
                DataFactory.list("agents?page=" + page, function(data) {
                    $scope.agents = data.list;
                    $scope.totalItems = data.totalItems;
                    $scope.currentPage = data.currentPage + 1;
                });
            }

            $scope.pageChanged = function() {
                ListAgents($scope.currentPage - 1);
            };

            ListAgents(0);

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
                            return { id: 0, name: '', towns: [] }
                        }
                    }
                });
                modalInstance.result.then(function(agent) {
                    var tempTowns = [];
                    for (var i = agent.towns.length - 1; i >= 0; i--) {
                        tempTowns.push({id: agent.towns[i].id, name: agent.towns[i].name});
                    }
                    agent.towns = tempTowns;
                    DataFactory.insert("agents", agent, function(data) { ListAgents(); });
                }, function() {
                    console.log('Modal dismissed at: ' + new Date());
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
                    var tempTowns = [];
                    for (var i = agent.towns.length - 1; i >= 0; i--) {
                        tempTowns.push({id: agent.towns[i].id, name: agent.towns[i].name});
                    }
                    agent.towns = tempTowns;
                    DataFactory.update("agents", agent.id, agent, function(data) {
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
                    console.log('Modal dismissed at: ' + new Date());
                });
            }

        }]);
}());
