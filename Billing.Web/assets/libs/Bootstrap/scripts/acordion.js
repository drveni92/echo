(function() {
    angular.module('bootDemo', ['ui.bootstrap']);

    AccordionDemoCtrl = function($scope) {

        $scope.oneAtATime = true;

        $scope.groups = [
            {
                title: 'Lorem Ipsum Group',
                content: 'Lorem ipsum dolor sit amet, pri et fierent indoctum, vim te dicta nusquam pertinacia. Ea rebum voluptua mel, ei qui nisl utamur abhorreant, aliquid nominavi et vix. Vis adhuc omnesque id. Cu vero reprimique vis, timeam feugiat quo in, et error laudem maiestatis quo.'
            },
            {
                title: 'Vim Modo Group Header',
                content: 'Vim modo insolens et. Ea lorem propriae vim, dicat partem usu ex. Et nisl impedit neglegentur his, vel ornatus minimum phaedrum in. Impedit fabellas salutandi pri an.'
            },
            {
                title: 'Quod Dicam Group Header',
                content: 'Quod dicam expetendis an nec, qui eu cibo graeci honestatis, copiosae abhorreant disputando cu qui. Vel in agam commodo nostrum, malorum argumentum pro ne. Mea illud ponderum ex. At iudico regione disputationi nec, nonumy aliquando at pri.'
            }
        ];

        $scope.items = ['Ana Balta', 'Amel Hadzic', 'Amina Imamovic'];

        $scope.addItem = function() {
            var newItemNo = $scope.items.length + 1;
            $scope.items.push('Clan tima broj ' + newItemNo);
        };

        $scope.status = {
            isFirstOpen: true,
            isFirstDisabled: false
        };

    };

    angular.controller("AccordionDemoCtrl", AccordionDemoCtrl);
}());
