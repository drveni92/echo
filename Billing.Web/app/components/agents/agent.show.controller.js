(function() {
    angular
        .module("Billing")
        .controller('AgentShowController', ['$scope','$route', '$routeParams', '$uibModal', 'DataFactory', function($scope, $route, $routeParams,$uibModal, DataFactory) {
            var agentId = $routeParams.id;

            if(agentId) {
                DataFactory.read("agents", agentId, function(data) {
                    $scope.agent = data;
                });
            }
            $('#profile-image1').on('click', function() {
                $('#profile-image-upload').click();
            });

            $scope.edit = function(agent) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    focus: false,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'app/components/agents/templates/changepassword.html',
                    controller: 'ModalInstanceController',
                    controllerAs: '$modal',
                    resolve: {
                        data: function() {
                            return $.extend(true, {}, agent)
                        }
                    }
                });
                modalInstance.result.then(function(agent) {

                    DataFactory.update("agents", agent.id, agent, function(data) {
                    });
                }, function() {
                });
            };



        }]);


}());