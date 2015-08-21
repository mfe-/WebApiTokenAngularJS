/// <reference path="scripts/typings/angularjs/angular.d.ts" />
/// <reference path="logincontroller.ts" />
/// <reference path="authenticationservice.ts" />
var app = (function () {
    function app() {
        var app = angular.module("app", ['ngRoute']);
        app.service("AuthenticationService", ["$rootScope", "$http", AuthenticationService]);
        this.App = app;
        //app.run(['$rootScope', '$route', 'AuthenticationService', '$location',
        //    function ($rootScope: ng.IRootScopeService, $route: angular.route.IRouteService, authenticationService: AuthenticationService, $location: ng.ILocationService) {
        //        $route.reload();
        //        $rootScope.$on("$locationChangeStart",(event: ng.IAngularEvent, args: any) => {
        //            if (authenticationService.IsAuthenticated == false) {
        //                $location.path('/login');
        //            }
        //        }).bind(this);
        //    }]);
    }
    return app;
})();
var a = new app();
a.App.controller("HomeController", ["$scope", "AuthenticationService", HomeController]);
a.App.controller("LoginController", ["$scope", "AuthenticationService", LoginController]);
a.App.config([
    '$routeProvider',
    function routes($routeProvider) {
        $routeProvider.when('/login', {
            templateUrl: '/login.html',
            name: "Home",
            resolve: {
                "AuthenticationService": ["AuthenticationService", function (authenticationService) {
                    return authenticationService.Resolve;
                }]
            }
        }).when('/home', {
            templateUrl: '/home.html',
            name: "Home"
        }).otherwise({
            redirectTo: '/login'
        });
    }
]);
//# sourceMappingURL=App.js.map