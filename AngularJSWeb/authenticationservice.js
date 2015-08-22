var AuthenticationService = (function () {
    function AuthenticationService($rootScope, $http, $location) {
        this.$location = $location;
        this.IsAuthenticated = false;
        this.scope = $rootScope;
        this.http = $http;
        $rootScope.$on("$locationChangeStart", this.LocationStartChangeEvent.bind(this));
    }
    AuthenticationService.prototype.login = function (login) {
        this.Resolve = this.http.post("http://localhost:5117/api/User/PostLogin", login);
        this.Resolve = this.Resolve.then(this.setLogin.bind(this));
        return this.Resolve;
    };
    AuthenticationService.prototype.setLogin = function (data, status, headers, config) {
        this.Token = data.data;
        this.http.defaults.headers.common['Authorization'] = this.Token;
        this.IsAuthenticated = true;
        this.scope.$broadcast("IsAuthenticatedEvent", { IsAuthenticated: this.IsAuthenticated, Token: this.Token });
    };
    AuthenticationService.prototype.LocationStartChangeEvent = function (event, args) {
        if (this.IsAuthenticated == false) {
            this.$location.path('/login');
        }
    };
    AuthenticationService.$inject = ["$rootScope", "$http"];
    return AuthenticationService;
})();
//# sourceMappingURL=authenticationservice.js.map