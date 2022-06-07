const _url = {};
var TempFilepath;
$(function () {
    
    _url.Get_select_M_REStaff = common.appPath + '/r_staff/Get_select_M_REStaff';
    _url.save_M_REStaff = common.appPath + '/r_staff/Save_M_REStaff';
    _url.check_Update_M_REStaff = common.appPath + '/r_staff/Check_Update_M_REStaff';
    setValidation();
    addEvents();
});

function setValidation() {

    $(".update-data").each(function (index) {
        var i = index + 1;

        $('#newProfilePhoto_' + i)
            .addvalidation_errorElement("#CheckImageFileError_" + i);

        $('#REStaffCD_' + i)
            .addvalidation_errorElement("#error_REStaffCD_" + i)
            .addvalidation_reqired()  //E101
            .addvalidation_maxlengthCheck(10) //E105
            .addvalidation_singlebyte_numberAlphabet(); //E104
           

        $('#REStaffName_' + i)
            .addvalidation_errorElement("#error_REStaffName_" + i)
            .addvalidation_reqired()  //E101
            .addvalidation_singlebyte_doublebyte(); //E105
           
            

        $('#REIntroduction_' + i)
            .addvalidation_errorElement("#error_REIntroduction_" + i)
            .addvalidation_singlebyte_doublebyte(); //E105
           
           

        $('#REPassword_' + i)
            .addvalidation_errorElement("#error_REPassword_" + i)
            .addvalidation_reqired()  //E101
            .addvalidation_onebyte_character() //E104
            .addvalidation_maxlengthCheck(20) //E105
            .addvalidation_minlengthCheck(8); //E110
          
    });
      //.addvalidation_checkisexist();//E314
}

function addEvents() {
    common.bindValidationEvent('#form1', '');
   
    $(document).on("change", ".uploadProfileInput", function () {
        var triggerInput = this;
        var currentImg = $(this).closest(".pic-holder").find(".pic").attr("src");
        var holder = $(this).closest(".pic-holder");
        var wrapper = $(this).closest(".profile-pic-wrapper");
        $(wrapper).find('[role="alert"]').remove();
        triggerInput.blur();
        var files = !!this.files ? this.files : [];
        if (!files.length || !window.FileReader) {
            return;
        }
        if (/^image/.test(files[0].type)) {
            // only image file
           
            var reader = new FileReader(); // instance of the FileReader
            reader.readAsDataURL(files[0]); // read the local file

            reader.onloadend = function () {
                //TempFilepath = this.result;
                $(holder).addClass("uploadInProgress");
                $(holder).find(".pic").attr("src", this.result);
                $(holder).append(
                    '<div class="upload-loader"><div class="spinner-border text-primary" role="status"><span class="sr-only">Loading...</span></div></div>'
                );

                // Dummy timeout; call API or AJAX below
                setTimeout(() => {
                    $(holder).removeClass("uploadInProgress");
                    $(holder).find(".upload-loader").remove();
                    // If upload successful
                    if (Math.random() < 0.9) {
                        $(wrapper).append(
                            '<div class="snackbar show" role="alert"><i class="fa fa-check-circle text-success"></i> Profile image updated successfully</div>'
                        );

                        // Clear input after upload
                        $(triggerInput).val("");
                        setTimeout(() => {
                            $(wrapper).find('[role="alert"]').remove();
                        }, 3000);
                    } else {
                        $(holder).find(".pic").attr("src", currentImg);
                        $(wrapper).append(
                            '<div class="snackbar show" role="alert"><i class="fa fa-times-circle text-danger"></i> There is an error while uploading! Please try again later.</div>'
                        );

                        // Clear input after upload
                        $(triggerInput).val("");
                        setTimeout(() => {
                            $(wrapper).find('[role="alert"]').remove();
                        }, 3000);
                    }
                }, 1500);
            };
        } else {
            $(wrapper).append(
                '<div class="alert alert-danger d-inline-block p-2 small" role="alert">' + common.getMessage("E304")+'</div>'
            );
           
            setTimeout(() => {
                $(wrapper).find('role="alert"').remove();
            }, 3000);
        }
    });

    
    $('#newStaffCD').on("change",function () {
        if ($('#newStaffCD').val().trim().length !== 0) {
            $('#newProfilePhoto')
                .addvalidation_errorElement("#CheckImageFileError");
            $('#newStaffCD')
                .addvalidation_errorElement("#newStaffCDError")
                .addvalidation_reqired()  //E101
                .addvalidation_maxlengthCheck(10) //E105
                .addvalidation_singlebyte_numberAlphabet();//E104

            $('#newStaffName')
                .addvalidation_errorElement("#newStaffNameError")
                .addvalidation_reqired()  //E101
                .addvalidation_singlebyte_doublebyte();//E105

            $('#newStaffIntro')
                .addvalidation_errorElement("#newStaffIntroError")
                .addvalidation_singlebyte_doublebyte();//E105

            $('#newStaffpsw')
                .addvalidation_errorElement("#newStaffpswError")
                .addvalidation_reqired()  //E101
                .addvalidation_onebyte_character() //E104
                .addvalidation_maxlengthCheck(20)  //E105
                .addvalidation_minlengthCheck(8); //E110
        }
        else
        {
            $('#newStaffCD')
                .removeValidation_required();
            $('#newStaffName')
                .removeValidation_required();

            $('#newStaffpsw')
                .removeValidation_required();
            $('#newStaffCD').hideError(); 
            $('#newProfilePhoto').hideError();
            $('#newStaffName').hideError();
            $('#newStaffIntro').hideError();
            $('#newStaffpsw').hideError();
        }
       

        const $this = $(this), $newStaffCD = $('#newStaffCD')
        if (!common.checkValidityInput($this)) {
            return false;
        }
        let model = {
            REStaffCD: $newStaffCD.val()
        };

        if (!model.REStaffCD) {
            $($newStaffCD).hideError();
            return;
        }

        common.callAjax(_url.Get_select_M_REStaff, model,
            function (result) {
                if (result && result.isOK) {
                    $($newStaffCD).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });

    $('#btnSaveChange').on('click', function () {
        $form = $('#form1').hideChildErrors();
        
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        //if (!common.checkValidityOnSave('#form1')) {
        //    $form.getInvalidItems().get(0).focus();
        //    return false;
        //}

        const $this = $(this), $newStaffCD = $('#newStaffCD')

        //alert($newStaffCD.val());
        //if ($newStaffCD.val() == "") {
        //    $('#modal-changesave').modal('show');
        //}


        if ($newStaffCD.val() == "" || $newStaffCD.val() == undefined) {
            let model = {};
            model.lst_StaffModel = Update_list_M_REStaff();
            Check_Update_M_REStaff(model, $form);
        }
        else {
        if ($('#newStaffCD').val().trim().length !== 0)
        {
            $('#newProfilePhoto')
                .addvalidation_errorElement("#CheckImageFileError");
            $('#newStaffCD')
                .addvalidation_errorElement("#newStaffCDError")
                .addvalidation_reqired()  //E101
                .addvalidation_maxlengthCheck(10) //E105
                .addvalidation_singlebyte_numberAlphabet();//E104

            $('#newStaffName')
                .addvalidation_errorElement("#newStaffNameError")
                .addvalidation_reqired()  //E101
                .addvalidation_singlebyte_doublebyte();//E105

            $('#newStaffIntro')
                .addvalidation_errorElement("#newStaffIntroError")
                .addvalidation_singlebyte_doublebyte();//E105

            $('#newStaffpsw')
                .addvalidation_errorElement("#newStaffpswError")
                .addvalidation_reqired()  //E101
                .addvalidation_onebyte_character() //E104
                .addvalidation_maxlengthCheck(20)  //E105
                .addvalidation_minlengthCheck(8); //E110
        }
        else {

            $('#newStaffCD')
                .removeValidation_required();
            $('#newStaffName')
                .removeValidation_required();
                
            $('#newStaffpsw')
                .removeValidation_required();
            $('#newStaffCD').hideError(); 
            $('#newProfilePhoto').hideError();
            $('#newStaffName').hideError();
            $('#newStaffIntro').hideError();
            $('#newStaffpsw').hideError();
            
            }
       
            let model = {
                REStaffCD: $newStaffCD.val()
            };

            if (!model.REStaffCD) {
                $($newStaffCD).hideError();
                return;
            }
            common.callAjax(_url.Get_select_M_REStaff, model,
                function (result) {
                    if (result && result.isOK) {
                        $($newStaffCD).hideError();
                        const data = result.data;
                        $('#modal-changesave').modal('show');
                    }
                    if (result && !result.isOK) {
                        const message = result.message;
                        $('#newStaffCD').showError(common.getMessage('E314'));
                    }
                });
        }
    });

    $('#btnOKChange').on('click', function () {
        
        const $REStaffCD = $("#newStaffCD").val(), $REStaffName = $('#newStaffName').val(), $REIntroduction = $('#newStaffIntro').val(), $REPassword = $('#newStaffpsw').val()
        let model = {
            REFaceImage: $("#newpic_holder").find("img").attr("src"),
            REStaffCD: $REStaffCD,
            REStaffName: $REStaffName,
            REIntroduction: $REIntroduction,
            REPassword: $REPassword,
            PermissionChat: $('#New_chkChat').is(':checked') ? "1" : "0",
            PermissionSetting: $('#New_chkSetting').is(':checked') ? "1" : "0",
            PermissionPlan: $('#New_chkPlan').is(':checked') ? "1" : "0",
            PermissionInvoice: $('#New_chkInvoice').is(':checked') ? "1" : "0",
        };

          model.lst_StaffModel = Update_list_M_REStaff();
          Save_M_REStaff(model, $form)
    });


    $('#CancleOK').on('click', function ()
    {
        window.location.reload();
    });

    $('#btnComplete').on('click', function () {
        window.location.reload();
    });
    $('#btnNoChange').on('click', function () {
        window.location.reload();
    });
    
}

function Update_list_M_REStaff() {
    let lst_update_StaffModel = [];
    $(".update-data").each(function (index) {
        var i = index + 1;
        const data = {
            REFaceImage:$('#newpic_holder_' + i).find("img").attr("src"),
            RealECD: $('#RealECD_' + i).val(),
            REStaffCD: $('#REStaffCD_' + i).val(),
            REStaffName: $('#REStaffName_' + i).val(),
            REIntroduction: $('#REIntroduction_' + i).val(),
            REPassword: $('#REPassword_' + i).val(),
            PermissionChat: $('#chkChat_' + i).is(':checked') ? "1" : "0",
            PermissionSetting: $('#chkSetting_' + i).is(':checked') ? "1" : "0",
            PermissionPlan: $('#chkPlan_' + i).is(':checked') ? "1" : "0",
            PermissionInvoice: $('#chkInvoice_' + i).is(':checked') ? "1" : "0",
        }
        lst_update_StaffModel.push(data);
    });

    return lst_update_StaffModel;
}
function Save_M_REStaff(model, $form) {
    common.callAjaxWithLoading(_url.save_M_REStaff, model, this , function (result) {
        if (result && result.isOK) {
            $('#modal-changeok').modal('show');
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    });
}

function Check_Update_M_REStaff(model, $form) {
    common.callAjaxWithLoading(_url.check_Update_M_REStaff, model, this, function (result) {
        if (result && result.isOK) {
            $('#modal-changesave').modal('show');
        }
        if (result && !result.isOK) {
            $('#modal-nochange').modal('show');
            //const errors = result.data;
            //for (key in errors) {
            //    const target = document.getElementById(key);
            //    $(target).showError(errors[key]);
            //    $form.getInvalidItems().get(0).focus();
            //}
        }
    });
}





