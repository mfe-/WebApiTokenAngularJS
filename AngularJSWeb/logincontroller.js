var LoginController = (function () {
    function LoginController($scope, $authenticationService) {
        this.scope = $scope;
        this.authenticationService = $authenticationService;
        this.Login = new Login();
    }
    LoginController.prototype.onClick = function () {
        this.authenticationService.login(this.Login);
    };
    LoginController.$inject = ["$scope", "AuthenticationService"];
    return LoginController;
})();
//# sourceMappingURL=logincontroller.js.map