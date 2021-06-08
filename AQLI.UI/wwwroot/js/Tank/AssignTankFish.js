var change_me = {
    init: function () {

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
    openModalExample: function () {
        // example - openModal('Fish/_AddMedicalRecord', fish.addMedicalRecordModalLoaded)
    },
    openModalConfirmExample: function (id) {
        //example - openConfirmModal('Purchases/_ConfirmDeletePurchase/?ID=' + id, PurchaseInvoice.openModalConfirmExample)
    },
    openModalConfirmExample: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred deleting the record.<br />Please refresh the page and try again.');
        }
    },
    modalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record.<br />Please refresh the page and try again.');
        }

        //On form submit using parsley
        $('#someExampleForm').parsley().on('form:submit', function () {



            //This return line is crucial to ensure the form does not do a regular(double) post
            return false;
        });
    }
}

$(change_me.init);