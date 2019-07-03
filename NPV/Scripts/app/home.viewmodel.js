function HomeViewModel(app, dataModel) {
    var self = this;

    self.npvRecords = ko.observableArray();

    self.allNpvRecords = ko.observableArray();

    Sammy(function () {
        this.get('#home', function () {
            // Make a call to the protected Web API by passing in a Bearer Authorization Header
            //$.ajax({
            //    method: 'get',
            //    url: app.dataModel.userInfoUrl,
            //    contentType: "application/json; charset=utf-8",
            //    headers: {
            //        'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
            //    },
            //    success: function (data) {
            //        self.myHometown('Your Hometown is : ' + data.hometown);
            //    }
            //});
        });
        this.get('/', function () { this.app.runRoute('get', '#home'); });
    });

    self.addCashFlow = function () {
        var cashFlowCount = $('#fieldContainer').find('.cash-flow').length;
        var cashFlowID = cashFlowCount + 1;
        var cashFlowName = "cashFlow" + cashFlowID;
        $("#fieldContainer").append("<div class='form-group cash-flow'><label class='control-label'>Cash Flow #" + cashFlowID + ":</label><input id='" + cashFlowName + "' maxlength='100' type='number' required='required' class='form-control' placeholder='Enter cash flow #" + cashFlowID + "' /></div>");
    };

    self.calculate = function () {
        self.calculateNPV();
    };

    self.calculateNPV = function () {
        var cashFlowCount = $('#fieldContainer').find('.cash-flow').length;
        var cashFlows = [];

        for (var i = 0; i < cashFlowCount; i++) {
            var cashFlowID = "#cashFlow" + (i + 1);
            var cashFlowValue = $(cashFlowID).val();

            cashFlows.push(cashFlowValue);
        }

        var request = {
            'InitialValue': $('#initialValue').val(),
            'LowerBoundDiscountRate': $('#lbDiscountRate').val(),
            'UpperBoundDiscountRate': $('#ubDiscountRate').val(),
            'DiscountRateIncrement': $('#discountRateIncrement').val(),
            'CashFlows': cashFlows
        };

        self.npvRecords.removeAll();

        $.ajax({
            method: 'post',
            data: JSON.stringify(request),
            url: '/api/Npv',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.npvRecords(data.npVs);
            },
            error: function () {

            }
        });
    };

    self.getAllResults = function () {

        self.allNpvRecords.removeAll();

        $.ajax({
            method: 'get',
            url: '/api/Npv/',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.allNpvRecords(data.npVs);
            },
            error: function () {

            }
        });
    };

    return self;
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});
