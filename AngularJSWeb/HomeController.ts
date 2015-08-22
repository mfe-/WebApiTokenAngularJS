class HomeController {

    static $inject = ["$scope", "$http"];

    protected scope: ng.IScope;
    protected authenticationService: AuthenticationService;

    constructor($scope: ng.IScope, private $http: ng.IHttpService) {
        this.scope = $scope;

        this.CurrentUser = this.$http.get("http://localhost:5117/api/User/GetLogin");

    }
    public CurrentUser: any;

} 