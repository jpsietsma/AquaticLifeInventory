var Maintenance = {
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

        if (!$.fn.DataTable.isDataTable('.MaintenanceLogTable')) {
            table = $('.MaintenanceLogTable').DataTable({               
                "searching": false,
                "pageLength": 10
            });

            //Attach click listener to rows trash icon to delete
            //$('#purchaseInvoicePurchaseTable tbody').on('click', 'i.icon-delete-row', function () {
            //    table
            //        .row($(this).parents('tr'))
            //        .remove()
            //        .draw();
            //});

            //Attach click listener to rows pencil icon to edit
            //$('#purchaseInvoicePurchaseTable tbody').on('click', 'i.icon-edit-row', function () {
            //    //show edit modal here, passing object
            //});

        }
    },
    events: {
        exampleButton_Clicked: function () {
            //example - Purchases.openModal(0);
            //could also pass in ID as function parameter and pass into modal for details if needed
        }                           
    },
    openLogDetailsModal: function (logType, logID) {
        if (logType == 1) {
            openModal('Maintenance/_Details_TemperatureLog/' + logID, Maintenance.temperatureLogModalLoaded)
        } else if (logType == 2) {
            openModal('Maintenance/_Details_InventoryLog/' + logID, Maintenance.inventoryLogModalLoaded)
        } else if (logType == 3) {
            openModal('Maintenance/_Details_WaterChangeLog/' + logID, Maintenance.waterChangeLogModalLoaded)
        }  else if (logType == 4) {
            openModal('Maintenance/_Details_FilterChangeLog/' + logID, Maintenance.filterChangeLogModalLoaded)
        } else if (logType == 5) {
            openModal('Maintenance/_Details_FeedingLog/' + logID, Maintenance.feedingLogModalLoaded)
        }      
    },
    temperatureLogModalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record.<br />Please refresh the page and try again.');
        }

        //On form submit using parsley
        $('#someExampleForm').parsley().on('form:submit', function () {



            //This return line is crucial to ensure the form does not do a regular(double) post
            return false;
        });
    },
    feedingLogModalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record.<br />Please refresh the page and try again.');
        }

        //On form submit using parsley
        $('#someExampleForm').parsley().on('form:submit', function () {



            //This return line is crucial to ensure the form does not do a regular(double) post
            return false;
        });
    },
    filterChangeLogModalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record.<br />Please refresh the page and try again.');
        }

        //On form submit using parsley
        $('#someExampleForm').parsley().on('form:submit', function () {



            //This return line is crucial to ensure the form does not do a regular(double) post
            return false;
        });
    },
    inventoryLogModalLoaded: function (success) {
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

$(Maintenance.init);