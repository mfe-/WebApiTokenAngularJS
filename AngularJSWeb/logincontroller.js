var LoginController = (function () {
    function LoginController($scope, $authenticationService, $location) {
        this.$location = $location;
        this.LoginFailed = false;
        this.scope = $scope;
        this.scope.$on("IsAuthenticatedEvent", this.IsAuthenticatedEvent.bind(this));
        this.authenticationService = $authenticationService;
        this.Login = new Login();
    }
    LoginController.prototype.onClick = function () {
        this.LoginFailed = false;
        var result = this.authenticationService.login(this.Login);
        result.catch(this.SetLoginFailed.bind(this));
    };
    LoginController.prototype.IsAuthenticatedEvent = function (event, args) {
        this.$location.path("/home");
    };
    LoginController.prototype.SetLoginFailed = function (data) {
        this.LoginFailed = true;
    };
    LoginController.$inject = ["$scope", "AuthenticationService", "$location"];
    return LoginController;
})();
//# sourceMappingURL=logincontroller.js.map