class LoginController {

    static $inject = ["$scope", "AuthenticationService"];

    protected scope: ng.IScope;
    protected authenticationService: AuthenticationService;

    constructor($scope: ng.IScope, $authenticationService: AuthenticationService) {
        this.scope = $scope;
        this.authenticationService = $authenticationService;
        this.Login = new Login();
    }
    public Login: Login;

    public onClick() {
        this.authenticationService.login(this.Login);
    }
} 