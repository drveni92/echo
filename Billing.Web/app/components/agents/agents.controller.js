(function() {
    angular
        .module("Billing")
        .controller('AgentsController', ['$scope', '$http', '$uibModal', 'DataFactory', 'AgentsFactory', 'ToasterService', function($scope, $http, $uibModal, DataFactory, AgentsFactory, ToasterService) {
            $scope.maxPagination = BillingConfig.maxPagination;

            $scope.pageParams = {
                page: 1,
                showPerPage: BillingConfig.showPerPage,
                sortType: 'name',
                sortReverse: false,
                totalItems: 0
            };

            function ListAgents() {
                $scope.pageParams.page = $scope.pageParams.page - 1;

                DataFactory.list("agents", function (data) {
                    $scope.agents = data.list;
                    $scope.pageParams.totalItems = data.totalItems;
                    $scope.pageParams.page = data.currentPage + 1;
                }, $scope.pageParams);
            }



            $scope.sort = function(column) {
                if($scope.pageParams.sortType === column) $scope.pageParams.sortReverse = !$scope.pageParams.sortReverse;
                $scope.pageParams.sortType = column;
                ListAgents();
            };

            $scope.search = function () {
                if ($scope.pageParams.name.toString().length > 2 || $scope.pageParams.name.toString().length === 0) ListAgents();
            };

            $scope.showItems = function () {
                ListAgents();
            };

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
                    templateUrl: 'app/view/agents/new.html',
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
                    templateUrl: 'app/view/agents/edit.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return $.extend(true, {}, agent);
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
                    templateUrl: 'app/view/agents/delete.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return agent;
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
            };

        }]);
}());
