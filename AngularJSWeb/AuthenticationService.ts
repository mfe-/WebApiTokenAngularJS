class AuthenticationService {
    static $inject = ["$rootScope", "$http"];

    protected scope: ng.IRootScopeService;
    protected http: ng.IHttpService;

    public IsAuthenticated: boolean = false;
    public Resolve: ng.IHttpPromise<{}>;

    constructor($rootScope: ng.IRootScopeService, $http: ng.IHttpService) {
        this.scope = $rootScope;
        this.http = $http;
    }
    public login(login:Login): void {
        this.http.post("http://localhost:5117/api/Default/PostLogin/", login);
    }
}