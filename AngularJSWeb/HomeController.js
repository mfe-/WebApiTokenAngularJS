var HomeController = (function () {
    function HomeController($scope, $http) {
        this.$http = $http;
        this.scope = $scope;
        this.CurrentUser = this.$http.get("http://localhost:5117/api/User/GetLogin");
    }
    HomeController.$inject = ["$scope", "$http"];
    return HomeController;
})();
//# sourceMappingURL=HomeController.js.map