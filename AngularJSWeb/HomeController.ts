class HomeController {

    static $inject = ["$scope", "$http"];

    protected scope: ng.IScope;
    protected authenticationService: AuthenticationService;

    constructor($scope: ng.IScope, private $http: ng.IHttpService) {
        this.scope = $scope;
        this.$http.get("http://localhost:5117/api/User/GetLogin").then(this.SetUserData.bind(this));

    }
    public CurrentUser: any;

    public SetUserData(data: ng.IHttpPromiseCallbackArg<any>, status: number, headers: ng.IHttpHeadersGetter, config: ng.IRequestConfig): void {
        this.CurrentUser = data.data;
    }

} 