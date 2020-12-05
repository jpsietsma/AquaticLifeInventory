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
        openNestedModal('Purchases/_PurchaseDetails/?ID=' + id, Purchases.modalLoaded)
    },
    openConfirmDeleteModal: function (id) {
        openConfirmModal('Purchases/_ConfirmDeletePurchase/?ID=' + id, Purchases.confirmModalLoaded)
    },
    confirmModalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred deleting the record.<br />Please refresh the page and try again.');
        }
    },
    DeletePurchase: function (id) {
        var purchaseId = id;

        console.log(purchaseId);

        $('[data-id="' + purchaseId + '"]').hide();

        $('#modalAddEdit').modal('toggle');
        $('#modalConfirm').modal('toggle');

    },
    modalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record.<br />Please refresh the page and try again.');
        }

        //Begin modal edit timeout
        var i = 10;

        var counterBack = setInterval(function () {
            i--;
            if (i > 0) {
                $('.progress-bar').css('width', i + '%');
            } else {
                clearTimeout(counterBack);
            }

        }, 1000);

        $('#addEditForm').parsley().on('form:submit', function () {
            //Send the form data to the server ajax
            saveFormDataPost($('#addEditForm'),
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