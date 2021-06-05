var fish = {
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
        addMedicalRecord_Clicked: function () {
            fish.openAddMedicalRecordModal();
        },        
        //purchaseQuantityOrCost_Changed: function () {

        //    //Change extended cost on quantity or cost update
        //    var quantityInputElement = document.getElementById('Quantity');
        //    var costInputElement = document.getElementById('Cost');
        //    var extCostInputElement = document.getElementById('ExtCost');

        //    $(extCostInputElement).val($(quantityInputElement).val() * $(costInputElement).val());

        //    //debugging
        //    //console.log("Quantity Updated: " + $(quantityInputElement).val() + " x " + $(costInputElement).val() + " = " + ($(quantityInputElement).val() * $(costInputElement).val()));           
        //},
    },
    openAddMedicalRecordModal: function () {
        openModal('/Fish/_AddMedicalRecord', fish.addMedicalRecordModalLoaded)
    },    
    //openConfirmDeleteModal: function (id) {
    //    openConfirmModal('Purchases/_ConfirmDeletePurchase/?ID=' + id, PurchaseInvoice.confirmDeleteModalLoaded)
    //},
    //confirmDeleteModalLoaded: function (success) {
    //    if (!success) {
    //        ErrorMessage.show('A problem has occurred deleting the record.<br />Please refresh the page and try again.');
    //    }
    //},     
    addMedicalRecordModalLoaded: function (success) {
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

$(fish.init);