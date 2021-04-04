var Tank = {
    init: function () {
        //Reload page on modal closing
        $('#modalAddEdit').on('hidden.bs.modal', function () {
            //location.reload();
        });

        $('#FishPurchaseTable').DataTable({
            "columnDefs": [
                {
                    "orderable": false,
                    "targets": [-1, -3]
                }
            ]
        });

        $('#SupplyPurchaseTable').DataTable({
            "columnDefs": [
                {
                    "orderable": false,
                    "targets": [-1, -3]
                }
            ]
        });

        $('#EquipmentPurchaseTable').DataTable({
            "columnDefs": [
                {
                    "orderable": false,
                    "targets": [-1, -3]
                }
            ]
        });
    },
    events: {
        addTank_Clicked: function () {
            Tank.openModal(0);
        },
        cardClicked: function (e) {
            var card = $(e);
            var id = $(card).attr("data-id");
            Tank.openModal(id);
        },
        confirmDelete_Clicked: function () {

        }
    },
    openModal: function (id) {
        openModal('Tank/_Details/?ID=' + id, Tank.modalLoaded)
    },
    openConfirmDeleteModal: function (id) {
        openModal('@Url.Action("_ConfirmDeleteTank")/?ID=' + id, Tank.confirmModalLoaded)
    },
    confirmModalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred deleting the record.<br />Please refresh the page and try again.');
        }
    },
    confirmModalLoaded: function (success) {

    },
    modalLoaded: function (success) {
        if (!success) {
            ErrorMessage.show('A problem has occurred obtaining the record.<br />Please refresh the page and try again.');
        }


        $('#addEditForm').parsley().on('form:submit', function () {
            //Send the form data to the server ajax
            saveFormDataPost($('#addEditForm'),
                function (data) {
                    //Success
                    $('#modalAddEdit').modal('hide');
                    if (data && data.View) {

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
    },
    HideCard: function (cardId) {
        $(cardId).hide();
        console.log(cardId);
    }
}

$(Tank.init);