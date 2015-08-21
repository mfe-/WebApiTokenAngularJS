class HomeController {

    static $inject = ["$scope", "AuthenticationService"];

    protected scope: ng.IScope;
    protected authenticationService: AuthenticationService;

    constructor($scope: ng.IScope, $authenticationService: AuthenticationService) {
        this.scope = $scope;
        this.authenticationService = $authenticationService;

    }

} 