var TankDashboard = {
    init: function () {                
        ////Reload page on modal closing
        //$('#modalAddEdit').on('hidden.bs.modal', function () {
        //    //location.reload();
        //});

        ////Open modal on purchase invoice cell click
        //$('.sdn-purchase-invoice-table-cell').on('click', function (e) {
        //    var id = $(e.target).closest("tr").attr("data-id");

        //    PurchaseInvoice.openPurchaseInvoiceModal(id);
        //});

        ////Initialize page tooltips
        //$('[data-toggle="tooltip"]').tooltip()

    },
    events: {
        //purchaseQuantityOrCost_Changed: function () {

        //    //Change extended cost on quantity or cost update
        //    var quantityInputElement = document.getElementById('Quantity');
        //    var costInputElement = document.getElementById('Cost');
        //    var extCostInputElement = document.getElementById('ExtCost');

        //    $(extCostInputElement).val($(quantityInputElement).val() * $(costInputElement).val());                
    },
    openAssignFishModal: function (id) {
        openModal('/Tank/_AddFish/' + id, TankDashboard.openAssignFishModalLoaded)
    },
    openTransferFishModal: function (id) {
        openModal('/Tank/_AddFish/' + id, TankDashboard.openAssignFishModalLoaded)
    },
    openAssignFishModalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record.<br />Please refresh the page and try again.');
        }

        $("#availableFish, #selectedFish").sortable({
            connectWith: ".assignedNewFish"
        }).disableSelection();

        $('#newFishForm').parsley().on('form:submit', function () {

            var newFishAssignments = [];

            $('#selectedFish').find('li').each(function () {
                var listId = $(this).attr("id");

                newFishAssignments.push(listId);
            });

            console.log(newFishAssignments);

            $("<input />")
                .attr("type", "hidden")
                .attr("name", "addedFishList")
                .attr("value", newFishAssignments)
                .appendTo('#newFishForm');

        });
    },
    openTransferFishModalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record.<br />Please refresh the page and try again.');
        }

        $("#availableFish, #selectedFish").sortable({
            connectWith: ".assignedNewFish"
        }).disableSelection();

        $('#newFishForm').parsley().on('form:submit', function () {

            var newFishAssignments = [];

            $('#selectedFish').find('li').each(function () {
                var listId = $(this).attr("id");

                newFishAssignments.push(listId);
            });

            console.log(newFishAssignments);

            $("<input />")
                .attr("type", "hidden")
                .attr("name", "addedFishList")
                .attr("value", newFishAssignments)
                .appendTo('#newFishForm');

        });
    }
}

$(TankDashboard.init);