/// <reference path="scripts/typings/angularjs/angular.d.ts" />
/// <reference path="logincontroller.ts" />
/// <reference path="authenticationservice.ts" />
class app {
    constructor() {
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
    public App: ng.IModule;
}
var a = new app();
a.App.controller("HomeController", ["$scope", "AuthenticationService", HomeController]);
a.App.controller("LoginController", ["$scope", "AuthenticationService", LoginController]);


a.App.config([
    <any>'$routeProvider', function routes($routeProvider: ng.route.IRouteProvider) {
        $routeProvider
            .when('/login',
            {
                templateUrl: '/login.html',
                name: "Home",
                resolve: {
                    "AuthenticationService": ["AuthenticationService", function (authenticationService: AuthenticationService) {
                        return authenticationService.Resolve;
                    }]
                }
            })
            .when('/home',
            {
                templateUrl: '/home.html',
                name: "Home"
            })
            .otherwise(
            {
                redirectTo: '/login'
            });
    }]);