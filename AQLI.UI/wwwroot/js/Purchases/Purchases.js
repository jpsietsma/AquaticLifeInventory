var Purchases = {
    init: function () {
        //make sure multiple modal backdrops overlay properly
        $(document).on('show.bs.modal', '.modal', function () {
            var zIndex = 1040 + (10 * $('.modal:visible').length);
            $(this).css('z-index', zIndex);
            setTimeout(function () {
                $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
            }, 0);
        });

        //Reload page on modal closing
        $('#modalAddEdit').on('hidden.bs.modal', function () {
            //location.reload();
        });

        //Open modal on purchase cell click
        $('.sdn-purchase-table-cell').on('click', function (e) {
            var id = $(e.target).closest("tr").attr("data-id");

            Purchases.openModal(id);
        });

        //Initialize page tooltips
        $('[data-toggle="tooltip"]').tooltip()
                                
    },
    events: {
        addPurchases_Clicked: function () {
            Purchases.openModal(0);
        },
        confirmDelete_Clicked: function () {

        },
        purchaseQuantityOrCost_Changed: function () {

            //Change extended cost on quantity or cost update
            var quantityInputElement = document.getElementById('Quantity');
            var costInputElement = document.getElementById('Cost');
            var extCostInputElement = document.getElementById('ExtCost');

            $(extCostInputElement).val($(quantityInputElement).val() * $(costInputElement).val());

            //debugging
            //console.log("Quantity Updated: " + $(quantityInputElement).val() + " x " + $(costInputElement).val() + " = " + ($(quantityInputElement).val() * $(costInputElement).val()));           
        },
    },
    openModal: function (id) {
        openModal('Purchases/_PurchaseDetails/?ID=' + id, Purchases.modalLoaded)
    },
    openPurchaseInvoiceModal: function (id) {
        openModal('Purchases/_PurchaseInvoiceDetails/?ID=' + id, Purchases.modalLoaded)
    },
    addPurchaseDetailsModal: function (id) {
        openNestedModal('Purchases/_PurchaseDetails/?ID=' + id, Purchases.nestedModalLoaded);
    },
    openConfirmDeleteModal: function (id) {
        openConfirmModal('Purchases/_ConfirmDeletePurchase/?ID=' + id, Purchases.confirmModalLoaded)
    },
    confirmModalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred deleting the record.<br />Please refresh the page and try again.');
        }
    },
    addPurchaseToInvoice: function () {
        console.log("hello!");      
    },
    DeletePurchase: function (id) {
        var purchaseId = id;

        console.log(purchaseId);

        $('[data-id="' + purchaseId + '"]').hide();

        $('#modalAddEdit').modal('toggle');
        $('#modalConfirm').modal('toggle');

    },
    nestedModalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred deleting the record.<br />Please refresh the page and try again.');
        }

        $('#addToInvoice').on('click', function () {
            var description = $('#PurchaseDescription').val();
            var quantity = $('#Quantity').val();
            var cost = $('#Cost').val();
            var extCost = $('#ExtCost').val();
            var purchaseCategory = $('#PurchaseCategoryID').val()


            $('#purchaseInvoicePurchaseTable').DataTable().row.add([
                    description,
                    quantity,
                    cost,
                    extCost,
                    purchaseCategory
                ]).draw(false);
            });
    },
    modalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record.<br />Please refresh the page and try again.');
        }

        ////Begin modal edit timeout
        //var i = 10;

        //var counterBack = setInterval(function () {
        //    i--;
        //    if (i > 0) {
        //        $('.progress-bar').css('width', i + '%');
        //    } else {
        //        clearTimeout(counterBack);
        //    }

        //}, 1000);
        $('[data-toggle="tooltip"]').tooltip();

        var table;

        if (!$.fn.DataTable.isDataTable('#purchaseInvoicePurchaseTable')) {
            table = $('#purchaseInvoicePurchaseTable').DataTable({
                "columns": [
                    null,
                    null,
                    null,
                    null,
                    { visible: false },
                    {
                        render: function (data, type, row) {
                            return '<a data-toggle="tooltip" data-placement="top" title="Remove Purchase" class="btn btn-outline-danger far fa-trash-alt icon-delete-row"></a> <a data-toggle="tooltip" data-placement="top" title="Edit" class="btn btn-outline-warning fas fa-pencil-alt icon-edit-row"></a>';
                        }
                    }
                ],
                "columnDefs": [
                    { orderable: false, "title": "Description", "targets": 0 },
                    { "title": "Cost", "targets": 1 },
                    { orderable: false, "title": "Quantity", "targets": 2 },
                    { orderable: false, "title": "Ext Cost", "targets": 3 },
                    { "title": "Category", "targets": 4 },
                    { orderable: false, "title": "", "targets": 5}
                ],
                "searching": false,
                "pageLength": 5,
                "oLanguage": {
                    "sEmptyTable": "No purchases added to invoice."
                }
            });

            //Attach click listener to rows trash icon to delete
            $('#purchaseInvoicePurchaseTable tbody').on('click', 'i.icon-delete-row', function () {
                table
                    .row($(this).parents('tr'))
                    .remove()
                    .draw();
            });

            //Attach click listener to rows pencil icon to edit
            $('#purchaseInvoicePurchaseTable tbody').on('click', 'i.icon-edit-row', function () {
                //show edit modal here, passing object
            });
            
        }

        $('#addPurchaseInvoiceForm').parsley().on('form:submit', function () {

            //Serialize the dataTable rows as a list of purchases - Working
            var purchaseTableData = table.rows().data();

            //Process the file upload

            //Send the form data to the server ajax to create the purcahse invoice
            saveFormDataWithFile($('#addPurchaseInvoiceForm'),
                function (data) {
                    //Success
                    $('#modalAddEdit').modal('hide');
                    if (data && data.View) {
                        location.reload();
                    } else {
                        ErrorMessage.show('An problem appears to have occurred saving the record. Please reload the page and try again.')
                    }
                },
                function (data) {
                    //Fail
                    if (typeof data.clientError != "undefined") {
                        ErrorMessage.show(data.clientError);
                    } else {
                        ErrorMessage.show(data.jsonResult);
                    }
                }
            );
            //saveFormDataPost($('#addPurchaseInvoiceForm'),
            //    function (data) {
            //        //Success
            //        $('#modalAddEdit').modal('hide');
            //        if (data && data.View) {
            //            location.reload();
            //        } else {
            //            ErrorMessage.show('An problem appears to have occurred saving the record. Please reload the page and try again.')
            //        }
            //    },
            //    function (data) {
            //        //Fail
            //        if (typeof data.clientError != "undefined") {
            //            ErrorMessage.show(data.clientError);
            //        } else {
            //            ErrorMessage.show(data.jsonResult);
            //        }
            //    }
            //);
            //This return line is crucial to ensure the form does not do a regular(double) post
            return false;
        });
    }
}

$(Purchases.init);