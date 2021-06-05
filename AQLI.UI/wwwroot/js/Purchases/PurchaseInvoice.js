var PurchaseInvoice = {
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

        //Open modal on purchase invoice cell click
        $('.sdn-purchase-invoice-table-cell').on('click', function (e) {
            var id = $(e.target).closest("tr").attr("data-id");

            PurchaseInvoice.openPurchaseInvoiceModal(id);
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
        openModal('Purchases/_PurchaseDetails/?ID=' + id, PurchaseInvoice.modalLoaded)
    },
    openPurchaseInvoiceModal: function (id) {
        openModal('Purchases/_PurchaseInvoiceDetails/?ID=' + id, PurchaseInvoice.modalLoaded)
    },
    addPurchaseDetailsModal: function (id) {
        openNestedModal('Purchases/_PurchaseDetails/?ID=' + id, PurchaseInvoice.nestedModalLoaded);
    },
    openConfirmDeleteModal: function (id) {
        openConfirmModal('Purchases/_ConfirmDeletePurchase/?ID=' + id, PurchaseInvoice.confirmModalLoaded)
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
            var purchaseID = $('#PurchaseID').val();
            var description = $('#PurchaseDescription').val();
            var cost = $('#Cost').val();
            var quantity = $('#Quantity').val();
            var extCost = $('#ExtCost').val();
            var purchaseCategory = $('#PurchaseCategoryID').val();
            var purchaseCategoryType = $('#PurchaseCategoryTypeID').val();


            $('#purchaseInvoicePurchaseTable').DataTable().row.add([
                purchaseID,
                description,
                cost,
                quantity,
                extCost,
                purchaseCategory,
                purchaseCategoryType
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
                    { visible: false },
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    {
                        render: function (data, type, row) {
                            return '<i data-toggle="tooltip" data-placement="top" title="Remove Purchase" class="btn btn-outline-danger far fa-trash-alt icon-delete-row"></i>';
                        }
                    }
                ],
                "columnDefs": [
                    { "title": "PurchaseID", "targets": 0 },
                    { "title": "Description", orderable: false, "targets": 1 },
                    { "title": "Cost", "targets": 2 },
                    { "title": "Quantity", orderable: false, "targets": 3 },
                    { "title": "Ext Cost", orderable: false, "targets": 4 },
                    { "title": "Category", "targets": 5 },
                    { "title": "Type", orderable: false, "targets": 6 },
                    { "title": "", orderable: false, "targets": 7 }
                ],
                "searching": false,
                "pageLength": 5,
                "oLanguage": {
                    "sEmptyTable": "No purchases currently added to this invoice."
                },
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };

                    // Total over all pages
                    total = api
                        .column(4)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                                        
                    // Update footer
                    $(api.column(4).footer()).html('$' + total);
                }
            });

            //Attach click listener to rows trash icon to delete
            $('#purchaseInvoicePurchaseTable tbody').on('click', 'i.icon-delete-row', function () {
                table
                    .row($(this).parents('tr'))
                    .remove()
                    .draw();
            });
                        
        }
                
        $('#addPurchaseInvoiceForm').parsley().on('form:submit', function () {

            //Retrieve purchases as an array from dataTable
            var purchaseData = table.rows().data();

            //Create array for purchase objects
            var purchaseArray = [];

            //Iterate through the purchase grid purchases and populate our Purchases json list
            $.each(purchaseData, function ($this, $propIdx, $propValue) {

                purchaseArray.push({
                    "PurchaseID": $propIdx[0],
                    "Quantity": $propIdx[3],
                    "Cost": $propIdx[2],
                    "Description": $propIdx[1],
                    "ExtCost": $propIdx[4],
                    "OwnerID": 0,
                    "StoreID": 0,
                    "PurchaseCategoryID": $propIdx[5],
                    "PurchaseCategoryTypeID": $propIdx[6],
                    "PurchaseDate": null
                });

            });

            //Send the form data to the server ajax to create the purcahse invoice and file upload - Working!
            savePurchaseFormDataWithInvoice($('#addPurchaseInvoiceForm'), purchaseArray,
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

            //clear the form, reset focus
            document.getElementById("#addPurchaseInvoiceForm").reset();

            //Reset focus on description for new add
            $("#PurchaseDescription").focus();

            //This return line is crucial to ensure the form does not do a regular(double) post
            return false;
        });
    }
}

$(PurchaseInvoice.init);