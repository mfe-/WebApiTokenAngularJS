class LoginController {

    static $inject = ["$scope", "AuthenticationService","$location"];

    protected scope: ng.IScope;
    protected authenticationService: AuthenticationService;

    constructor($scope: ng.IScope, $authenticationService: AuthenticationService, private $location: ng.ILocationService) {
        this.scope = $scope;
        this.scope.$on("IsAuthenticatedEvent", this.IsAuthenticatedEvent.bind(this));
        this.authenticationService = $authenticationService;
        this.Login = new Login();
    }
    public Login: Login;

    public onClick() {
        this.authenticationService.login(this.Login);
    }
    public IsAuthenticatedEvent(event: ng.IAngularEvent, args: any[]) {
        this.$location.path("/home");
    }
} 