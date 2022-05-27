const _url = {};
$(function () {
    setValidation();
    _url.Get_select_M_REStaff = common.appPath + '/r_staff/Get_select_M_REStaff';
    //_url.InsertL_Log = common.appPath + '/r_asmc_ms_reged_list/Insert_l_log';
    addEvents();
    //$('#MansionName').focus();
});

function setValidation() {
    $('#newProfilePhoto')
        .addvalidation_errorElement("#CheckImageFileError");

    $('#newStaffCD')
        .addvalidation_errorElement("#newStaffCDError")
        .addvalidation_reqired()  //E101
        .addvalidation_onebyte_character() //E104
        .addvalidation_maxlengthCheck(10); //E105

    $('#newStaffName')
        .addvalidation_errorElement("#newStaffNameError")
        .addvalidation_reqired()  //E101
        .addvalidation_maxlengthCheck(30); //E105


    $('#newStaffIntro')
        .addvalidation_errorElement("#newStaffIntroError")
        .addvalidation_maxlengthCheck(500); //E105

    $('#newStaffpsw')
        .addvalidation_errorElement("#newStaffpswError")
        .addvalidation_reqired()  //E101
        .addvalidation_onebyte_character() //E104
        .addvalidation_minlengthCheck(8)   //E110
        .addvalidation_maxlengthCheck(20); //E105

    $('#btnSaveChange')
        .addvalidation_errorElement("#btnSaveChangeError")
}

function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#newProfilePhoto').on('change', function () {
        const $this = $(this)

        if (!common.checkValidityInput($this)) {
            return false;
        }
        var fileInput = document.getElementById('#newProfilePhoto');
        var filePath = fileInput.value;
        var allowedExtensions = /(\.jpg|\.jpeg|\.png|\.gif)$/i;
        if (!allowedExtensions.exec(filePath)) {
            alert('Please upload file having extensions .jpeg/.jpg/.png/.gif only.');
            fileInput.value = '';
            return false;
        } else {
            //Image preview
            if (fileInput.files && fileInput.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('imagePreview').innerHTML = '<img src="' + e.target.result + '"/>';
                };
                reader.readAsDataURL(fileInput.files[0]);
            }
        }
    });
    
    $('#newStaffCD').on('change', function () {
        const $this = $(this), $newStaffCD = $('#newStaffCD')

        if (!common.checkValidityInput($this)) {
            return false;
        }
        let model = {
            REStaffCD: $newStaffCD.val()
        };

        common.callAjax(_url.Get_select_M_REStaff, model, function (result) {
            if (result && result.isOK) {
                $('#newStaffCD').hideError();
            }
            if (result && !result.isOK) {
                if (result.message != null) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                    $('#newStaffCD').showError(common.getMessage('E314'));
                }
                //else {
                //    const errors = result.data;
                //    for (key in errors) {
                //        const target = document.getElementById(key);
                //        $(target).showError(errors[key]);
                //        $form.getInvalidItems().get(0).focus();
                //    }
                //}
            }
        });
    });
}


function fileValidation() {
    var fileInput = document.getElementById('file');
    var filePath = fileInput.value;
    var allowedExtensions = /(\.jpg|\.jpeg|\.png|\.gif)$/i;
    if (!allowedExtensions.exec(filePath)) {
        alert('Please upload file having extensions .jpeg/.jpg/.png/.gif only.');
        fileInput.value = '';
        return false;
    } else {
        //Image preview
        if (fileInput.files && fileInput.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById('imagePreview').innerHTML = '<img src="' + e.target.result + '"/>';
            };
            reader.readAsDataURL(fileInput.files[0]);
        }
    }
}