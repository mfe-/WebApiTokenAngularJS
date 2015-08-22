class AuthenticationService {
    static $inject = ["$rootScope", "$http"];

    protected scope: ng.IRootScopeService;
    protected http: ng.IHttpService;

    public IsAuthenticated: boolean = false;
    public Resolve: ng.IPromise<{}>;
    public Token: string;

    constructor($rootScope: ng.IRootScopeService, $http: ng.IHttpService, private $location: ng.ILocationService) {
        this.scope = $rootScope;
        this.http = $http;

        $rootScope.$on("$locationChangeStart", this.LocationStartChangeEvent.bind(this));
    }
    public login(login: Login): void {
        this.Resolve = this.http.post("http://localhost:5117/api/User/PostLogin", login);

        this.Resolve = this.Resolve.then(this.setLogin.bind(this));
    }

    public setLogin(data: ng.IHttpPromiseCallbackArg<string>, status: number, headers: ng.IHttpHeadersGetter, config: ng.IRequestConfig): void {
        this.Token = data.data;
        this.http.defaults.headers.common['Authorization'] = this.Token;
        this.IsAuthenticated = true;
        this.scope.$broadcast("IsAuthenticatedEvent", { IsAuthenticated: this.IsAuthenticated, Token: this.Token });
    }
    public LocationStartChangeEvent(event: ng.IAngularEvent, args: any[]) {

        if (this.IsAuthenticated == false) {
            this.$location.path('/login');
        }
    }
}