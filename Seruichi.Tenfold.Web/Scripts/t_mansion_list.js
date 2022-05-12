const _url = {};

$(function () {
    setValidation();
    _url.GetM_MansionList = common.appPath + '/t_mansion_list/GetM_MansionList';
    addEvents();
});

function setValidation() {
    
    $('#StartNum')
        .addvalidation_errorElement("#errorAge")
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(3)
        .addvalidation_numcompare();

    $('#EndNum')
        .addvalidation_errorElement("#errorAge")
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(3)
        .addvalidation_numcompare();

    $('#StartUnit')
        .addvalidation_errorElement("#errorUnit")
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(3);


    $('#EndUnit')
        .addvalidation_errorElement("#errorUnit")
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(3);

}

function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#StartNum, #EndNum').on('change', function () {

        const $this = $(this), $start = $('#StartNum').val(), $end = $('#EndNum').val()

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            StartAge: $start,
            EndAge: $end
        };

        if (model.StartAge && model.EndAge) {
            if (model.StartAge < model.EndAge) {
                $("#StartNum").hideError();
                $("#EndNum").hideError();
                $("#EndNum").focus();
                return;
            }

        }

    });

    $('#StartUnit, #EndUnit').on('change', function () {
        const $this = $(this), $start1 = $('#StartUnit').val(), $end1 = $('#EndUnit').val()

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            StartUnit: $start1,
            EndUnit: $end1
        };

        if (model.StartUnit && model.EndUnit) {
            if (model.StartUnit > model.EndUnit) {
                $("#StartUnit").showError(common.getMessage('E113'));
                //$("#EndUnit").showError(this.getMessage('E113'));
                $("#StartUnit").focus();
                return;
            }
            else {
                $("#StartUnit").hideError();
                $("#EndUnit").hideError();
                $("#EndUnit").focus();
                return;
            }

        }

    });

    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        $('#mansiontable tbody').empty();

        const $Apartment = $("#txtApartment").val(), $StartAge = $("#StartNum").val(), $EndAge = $('#EndNum').text(),
            $StartUnit = $("#StartUnit").val(), $EndUnit = $("#EndUnit").val()

        if ($StartAge )
          
        let model = {
            Apartment: $Apartment,
            SellerName: $StartAge,
            RangeSelect: $EndAge,
            StartDate: $StartUnit,
            EndDate: $EndUnit,
        };
        GetM_MansionList(model, $form);

    });


}