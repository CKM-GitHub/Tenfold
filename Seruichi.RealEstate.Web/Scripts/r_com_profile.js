function setValidation() {
    $('#txtBusinessHours')
        .addvalidation_errorElement("#errorBusinessHours")
        .addvalidation_maxlengthCheck(25)//E105
        .addvalidation_doublebyte();

    $('#txtCompanyHoliday')
        .addvalidation_errorElement("#errorCompanyHoliday")
        .addvalidation_maxlengthCheck(50);

    $('#txtPassword')
        .addvalidation_errorElement("#errorPassword")
        .addvalidation_reqired()
        .addvalidation_singlebyte()
        .addvalidation_passwordcompare()
        .addvalidation_minlengthCheck(8)
        .addvalidation_maxlengthCheck(20);

    $('#txtConfirmPassword')
        .addvalidation_errorElement("#errorConfirmPassword")
        .addvalidation_reqired()
        .addvalidation_singlebyte()
        .addvalidation_passwordcompare()
        .addvalidation_maxlengthCheck(20);
}

function addEvents(model) {
    common.bindValidationEvent('#maindiv', '');

    if (model.LoginID !== 'admin') {
        $('#adminblock').addClass('d-none');
    }

    if (model.PermissionSetting !== '1') {
        $('#txtBusinessHours').attr('readonly', true);
        $('#txtCompanyHoliday').attr('readonly', true);
        $('#txtPassword').attr('readonly', true);
        $('#txtConfirmPassword').attr('readonly', true);
        $('#btnConfirm').focus();
    }
    else {
        $('#txtBusinessHours').focus();
    }

    Get_ModalData(model.RealECD);

    $('#btnConfirm').on('click', function () {
        $form = $('#maindiv').hideChildErrors();
        if (!common.checkValidityOnSave('#maindiv')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        Bind_ModalData();
        $('#ConfirmModal').modal('show');
    });

    $('#btnProcess').on('click', function () {
        Modified_M_RealEstate_Data();
    });

    $('#btnxC_alert').on('click', function () {
        window.location.reload();
    });

    $('#btnC_alert').on('click', function () {
        window.location.reload();
    });

    $('#btnDestruction').on('click', function () {
        window.location.reload();
    })
}

function Get_ModalData(RealECD) {
    var model = {
        RealECD: RealECD
    }
    common.callAjaxWithLoading(_url.get_r_com_profileData, model, this, function (result) {
        if (result && result.isOK) {
            Bind_DetailData(JSON.parse(result.data));
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

function Bind_DetailData(data) {
    if (data.length !== 0) {
        //Main Form
        $('#txtREName').val(data[0]['REName']);
        $('#txtREKana').val(data[0]['REKana']);
        $('#txtPresident').val(data[0]['President']);
        $('#txtJoinedDate').val(data[0]['JoinedDate']);
        $('#txtZipCode1').val(data[0]['ZipCode1']);
        $('#txtZipCode2').val(data[0]['ZipCode2']);
        $('#txtPrefName').val(data[0]['PrefName']);
        $('#txtCity_Town').val(data[0]['CityName'] + data[0]['TownName']);
        $('#txtAddress1').val(data[0]['Address1']);
        $('#txtAddress2').val(data[0]['Address2']);

        $('#txtPICName').val(data[0]['PICName']);
        $('#txtPICKana').val(data[0]['PICKana']);
        $('#txtHousePhone').val(data[0]['HousePhone']);
        $('#txtFax').val(data[0]['Fax']);
        $('#txtMailAddress').val(data[0]['MailAddress']);
        $('#txtBusinessHours').val(data[0]['BusinessHours']);
        $('#txtCompanyHoliday').val(data[0]['CompanyHoliday']);
        $('#txtBankName').val(data[0]['BankName']);
        $('#txtBranchName').val(data[0]['BranchName']);
        $('#txtSourceAccountType').val(data[0]['SourceAccountType']);
        $('#txtSourceAccount').val(data[0]['SourceAccount']);
        $('#txtSourceAccountName').val(data[0]['SourceAccountName']);

        $('#txtLicenceNo1').val(data[0]['LicenceNo1']);
        $('#txtLicenceNo2').val(data[0]['LicenceNo2']);
        $('#txtLicenceNo3').val(data[0]['LicenceNo3']);
        $('#txtPassword').val(data[0]['Password']);
        $('#txtConfirmPassword').val(data[0]['Password']);

        //Modal
        $('#m_txtREName').val(data[0]['REName']);
        $('#m_txtREKana').val(data[0]['REKana']);
        $('#m_txtPresident').val(data[0]['President']);
        $('#m_txtJoinedDate').val(data[0]['JoinedDate']);
        $('#m_txtZipCode1').val(data[0]['ZipCode1']);
        $('#m_txtZipCode2').val(data[0]['ZipCode2']);
        $('#m_txtPrefName').val(data[0]['PrefName']);
        $('#m_txtCity_Town').val(data[0]['CityName'] + data[0]['TownName']);
        $('#m_txtAddress1').val(data[0]['Address1']);
        $('#m_txtAddress2').val(data[0]['Address2']);

        $('#m_txtPICName').val(data[0]['PICName']);
        $('#m_txtPICKana').val(data[0]['PICKana']);
        $('#m_txtHousePhone').val(data[0]['HousePhone']);
        //$('#m_txtCellPhone').val('*****');
        $('#m_txtFax').val(data[0]['Fax']);
        $('#m_txtMailAddress').val(data[0]['MailAddress']);
        $('#m_txtBankName').val(data[0]['BankName']);
        $('#m_txtBranchName').val(data[0]['BranchName']);
        $('#m_txtSourceAccountType').val(data[0]['SourceAccountType']);
        $('#m_txtSourceAccount').val(data[0]['SourceAccount']);
        $('#m_txtSourceAccountName').val(data[0]['SourceAccountName']);
        $('#m_txtLicenceNo1').val(data[0]['LicenceNo1']);
        $('#m_txtLicenceNo2').val(data[0]['LicenceNo2']);
        $('#m_txtLicenceNo3').val(data[0]['LicenceNo3']);
    }
}

function Bind_ModalData() {
    $('#m_txtBusinessHours').val($('#txtBusinessHours').val());
    $('#m_txtCompanyHoliday').val($('#txtCompanyHoliday').val());
}

function Modified_M_RealEstate_Data() {
    var model = {
        BusinessHours: $('#txtBusinessHours').val(),
        CompanyHoliday: $('#txtCompanyHoliday').val(),
        Password: $('#txtPassword').val()
    }

    common.callAjaxWithLoading(_url.update_r_com_profileData, model, this, function (result) {
        if (result && result.isOK) {
            Insert_l_log();
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

function Insert_l_log() {
    let model = {
        LoginKBN: 2,
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        PageID: 'r_com_profile',
        ProcessKBN: 'Update',
        Remarks: '業者情報の更新'
    };
    common.callAjax(_url.insert_l_log, model,
        function (result) {
            if (result && result.isOK) {
                $('#SaveCompleteModal').modal('show');
            }
            if (result && !result.isOK) {
            }
        });
}