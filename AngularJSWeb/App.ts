/// <reference path="scripts/typings/angularjs/angular.d.ts" />
/// <reference path="logincontroller.ts" />
/// <reference path="authenticationservice.ts" />
class app {
    constructor() {
        var app = angular.module("app", ['ngRoute']);

        app.service("AuthenticationService", ["$rootScope", "$http","$location", AuthenticationService]);

        this.App = app;
        app.run(['$rootScope', '$route', 'AuthenticationService', '$location',
            function ($rootScope: ng.IRootScopeService, $route: angular.route.IRouteService, authenticationService: AuthenticationService, $location: ng.ILocationService) {
                $route.reload();
                if (authenticationService.IsAuthenticated == false) {
                    $location.path('/login');
                }
            }]);
    }
    public App: ng.IModule;
}
var a = new app();

a.App.controller("HomeController", ["$scope", "$http", HomeController]);
a.App.controller("LoginController", ["$scope", "AuthenticationService", "$location", LoginController]);

a.App.config([
    <any>'$routeProvider', function routes($routeProvider: ng.route.IRouteProvider) {
        $routeProvider
            .when('/login',
            {
                templateUrl: '/login.html',
                resolve: {
                    "AuthenticationService": ["AuthenticationService", function (authenticationService: AuthenticationService) {
                        return authenticationService.Resolve;
                    }]
                }
            })
            .when('/home',
            {
                templateUrl: '/home.html'
            })
            .otherwise(
            {
                redirectTo: '/login'
            });
    }]);

