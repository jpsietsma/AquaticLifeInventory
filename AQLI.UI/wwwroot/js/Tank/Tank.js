var Tank = {
    init: function () {
        Tank.bindtable(); // call this so we can have one code call once we get binding working
    },
    events: {
        addTank_Clicked: function () {
            Tank.openModal(0);
        },
        cardClicked: function (e) {
            var card = $(e);
            //if (!$(row).is('tr')) {
            //    row = $(row).closest('tr');
            //}
            var id = $(card).attr("data-id");
            Tank.openModal(id);
        }
    },
    openModal: function (id) {
        openModal('@Url.Action("_Details")/?ID=' + id, Tank.modalLoaded)
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
                        $('#resultsDiv').html(data.View).fadeIn();
                        Tank.bindtable(); // call this so we can have one code call once we get binding working
                    }
                    else {
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
    },
    bindtable: function () {
        //$('#contactTypeTable').bindTablesorter();
    }
}

$(Tank.init);