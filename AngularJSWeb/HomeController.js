var HomeController = (function () {
    function HomeController($scope, $http) {
        this.$http = $http;
        this.scope = $scope;
        this.$http.get("http://localhost:5117/api/User/GetLogin").then(this.SetUserData.bind(this));
    }
    HomeController.prototype.SetUserData = function (data, status, headers, config) {
        this.CurrentUser = data.data;
    };
    HomeController.$inject = ["$scope", "$http"];
    return HomeController;
})();
//# sourceMappingURL=HomeController.js.map