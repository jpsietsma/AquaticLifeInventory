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

        //Open modal on purchase invoice cell click
        $('.sdn-purchase-invoice-table-cell').on('click', function (e) {
            var id = $(e.target).closest("tr").attr("data-id");

            Purchases.openPurchaseInvoiceModal(id);
        });

        //Initialize page tooltips
        $('[data-toggle="tooltip"]').tooltip({
            delay: { show: 0, hide: 200 }
        });
                                
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

        },
        purchaseType_Changed: function () {

            $("#PurchaseCategoryTypeID").on("change", function () {
                var sel = $(this).val(),
                    $ddl = $("#PurchaseCategoryID");
                if (!sel) { // if there is no value means first option is selected
                    $ddl.find("option").show();
                    $ddl.val(''); // show all options and reset the value
                }
                else {
                    $ddl.find("option").hide();

                    // hide all options
                    // show only those options which contain the selected value, 
                    // set the selected property to true for the only remaining ones
                    $ddl.find("option[data-categoryType*='" + sel + "']").show().prop('selected', true);
                }
            });

        }
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

        $ddl = $("#PurchaseCategoryID").find("option").hide();

        $("#PurchaseCategoryTypeID").on("change", function () {
            var sel = $(this).val(),
                $ddl = $("#PurchaseCategoryID");
                $ddl.find("option").hide();
                $ddl.find("option[data-categoryType='" + sel + "']")
            if (!sel) { // if there is no value means first option is selected
                $ddl.find("option[data-categoryType='" + sel + "']").show();
                $ddl.val(''); // show all options and reset the value
            }
            else {
                $ddl.find("option").hide();

                // hide all options
                // show only those options which contain the selected value, 
                // set the selected property to true for the only remaining ones
                $ddl.find("option[data-categoryType='" + sel + "']").show().prop('selected', true);
            }
        });

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

            /* Reset purchase form and set focus to Purchase Description field */
            document.getElementById("addEditPurchaseForm").reset();
            $("#PurchaseDescription").focus();
        });

    },
    modalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record.<br />Please refresh the page and try again.');
        }

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
                    { visible: false },
                    { visible: false },
                    {
                        render: function (data, type, row) {
                            return '<i data-toggle="tooltip" data-placement="top" title="Remove Purchase" class="btn btn-outline-danger far fa-trash-alt icon-delete-row"></i> <i data-toggle="tooltip" data-placement="top" title="Edit" class="btn btn-outline-warning fas fa-pencil-alt icon-edit-row"></i>';
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
                    { "title": "Type", "targets": 6},
                    { "title": "", orderable: false, "targets": 7}
                ],
                "searching": false,
                "pageLength": 5,
                "oLanguage": {
                    "sEmptyTable": "No purchases added to invoice."
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

            //Attach click listener to rows pencil icon to edit
            $('#purchaseInvoicePurchaseTable tbody').on('click', 'i.icon-edit-row', function () {
                //show edit modal here, passing object
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
            
            //This return line is crucial to ensure the form does not do a regular(double) post
            return false;
        });
    }
}

$(Purchases.init);

//Add purchase invoice javascript here for simplicity in accessing within a tab
