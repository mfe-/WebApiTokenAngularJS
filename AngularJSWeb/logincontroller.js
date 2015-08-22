var LoginController = (function () {
    function LoginController($scope, $authenticationService, $location) {
        this.$location = $location;
        this.scope = $scope;
        this.scope.$on("IsAuthenticatedEvent", this.IsAuthenticatedEvent.bind(this));
        this.authenticationService = $authenticationService;
        this.Login = new Login();
    }
    LoginController.prototype.onClick = function () {
        this.authenticationService.login(this.Login);
    };
    LoginController.prototype.IsAuthenticatedEvent = function (event, args) {
        this.$location.path("/home");
    };
    LoginController.$inject = ["$scope", "AuthenticationService", "$location"];
    return LoginController;
})();
//# sourceMappingURL=logincontroller.js.map