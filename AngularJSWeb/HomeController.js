var HomeController = (function () {
    function HomeController($scope, $authenticationService) {
        this.scope = $scope;
        this.authenticationService = $authenticationService;
    }
    HomeController.$inject = ["$scope", "AuthenticationService"];
    return HomeController;
})();
//# sourceMappingURL=HomeController.js.map