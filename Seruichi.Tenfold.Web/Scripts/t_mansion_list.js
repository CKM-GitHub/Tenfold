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

        const $Apartment = $("#txtApartment").val(), $StartAge = $("#StartNum").val(), $EndAge = $('#EndNum').val(),
            $StartUnit = $("#StartUnit").val(), $EndUnit = $("#EndUnit").val()

        let model = {
            Apartment: $Apartment,
            StartAge: Get_FT_Age($EndAge, 'F'),
            EndAge: Get_FT_Age($StartAge, 'T'),
            StartDate: $StartUnit,
            EndDate: $EndUnit,
        };
        GetM_MansionList(model, $form);

    });


}

function Get_FT_Age(age, type) {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    var start_yyyymm = '', end_yyyymm = '';

    //Forｍ.築年数（To） => 築年月(From)
    if (type == 'F') {
        if (age !== '') {
            start_yyyymm = (yyyy - age) + '/' + String(parseInt(mm) + 1).padStart(2, '0');
        }
        return start_yyyymm;
    }
    //Forｍ.築年数（From） => 築年月(To)
    else if (type == 'T') {
        if (age !== '') {
            end_yyyymm = (yyyy - (age - 1)) + '/' + mm;
        }
        return end_yyyymm;
    }
}