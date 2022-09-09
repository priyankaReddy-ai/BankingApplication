
GetAllAccounts();
function GetAllAccounts() {
    alert()
    dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "../Home/GetAllAccounts",
                cache: false,
                dataType: "Json"
            },
            parameterMap: function (data, operation) {
                return data;
            },
        },
        requestStart: function () {
            // alert("request start");
            ShowProgress();
        },
        requestEnd: function () {
            // alert("request end");
            hideload();

        },
        pageSize: 10,
        batch: true,

        schema: {
            model: {
                id: "AccountID",
                fields: {
                    AccountID: {  readonly: true },
                    AccountName: { validation: { required: false } },
                    Balance: { validation: { required: false } },
                }
            }
        },
        
    });

   

    $("#accountGrid").kendoGrid({
        dataSource: dataSource,
        pageable: {
            refresh: true,
            pageSizes: true,
            pageSize: 10,
        },
        sortable: true,
        filterable: true,

        columns: [
            { field: "AccountID", hidden: true },
            
            {
                field: "AccountName",
                width: "19%", title: "Account Name"
            },
            { field: "Balance", title: "Balance", width: "19%" },
           
            //{
            //    title: "Actions",
            //    template: "<input type='button' onclick='deleteRecord(#:ClientID#)' data-target='.box' data-toggle='modal' value='Deactive' class='btnn' />",
            //    width: "10%"
            //},
            // { field: "isWording", title: "Is Wording", hidden: true },

        ],
    });
    $(".k-grid-add").on("click", function () {
        isCreating = true;
    });
}