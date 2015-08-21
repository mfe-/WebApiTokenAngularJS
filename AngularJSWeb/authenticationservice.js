var AuthenticationService = (function () {
    function AuthenticationService($rootScope, $http) {
        this.IsAuthenticated = false;
        this.scope = $rootScope;
        this.http = $http;
    }
    AuthenticationService.prototype.login = function (login) {
        this.http.post("http://localhost:5117/api/Default/PostLogin/", login);
    };
    AuthenticationService.$inject = ["$rootScope", "$http"];
    return AuthenticationService;
})();
//# sourceMappingURL=authenticationservice.js.map