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
        }        
    },
    openAddMedicalRecordModal: function () {
        openModal('/Fish/_AddMedicalRecord', fish.addMedicalRecordModalLoaded)
    },
    openFishDetailsModal: function (id) {
        openModal('/Fish/Details/' + id, fish.fishDetailsModalLoaded)
    },
    fishDetailsModalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record. <br />Please refresh the page and try again.')
        }
    },
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