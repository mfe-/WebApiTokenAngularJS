/// <reference path="scripts/typings/angularjs/angular.d.ts" />
/// <reference path="logincontroller.ts" />
/// <reference path="authenticationservice.ts" />
var app = (function () {
    function app() {
        var app = angular.module("app", ['ngRoute']);
        app.service("AuthenticationService", ["$rootScope", "$http", "$location", AuthenticationService]);
        this.App = app;
        app.run(['$rootScope', '$route', 'AuthenticationService', '$location', function ($rootScope, $route, authenticationService, $location) {
            $route.reload();
            if (authenticationService.IsAuthenticated == false) {
                $location.path('/login');
            }
        }]);
    }
    return app;
})();
var a = new app();
a.App.controller("HomeController", ["$scope", "$http", HomeController]);
a.App.controller("LoginController", ["$scope", "AuthenticationService", "$location", LoginController]);
a.App.config([
    '$routeProvider',
    function routes($routeProvider) {
        $routeProvider.when('/login', {
            templateUrl: '/login.html',
            resolve: {
                "AuthenticationService": ["AuthenticationService", function (authenticationService) {
                    return authenticationService.Resolve;
                }]
            }
        }).when('/home', {
            templateUrl: '/home.html'
        }).otherwise({
            redirectTo: '/login'
        });
    }
]);
//# sourceMappingURL=App.js.map