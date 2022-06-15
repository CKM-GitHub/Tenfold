const _url = {};
$(function () {
    _url.get_t_seller_Info = common.appPath + '/t_seller_account/get_t_seller_Info';
    _url.insert_M_Seller = common.appPath + '/t_seller_account/insert_M_Seller';
    addEvents()
});

function addEvents() {
    $('#reale').addClass('d-none');
    $('#submenu_reale').addClass('d-none');
    Bind_Company_Data(this);
    let model1 = {
        SellerCD: common.getUrlParameter('SellerCD')
    }
    get_account_Data(model1,this);

    $('#chkTestFLG').on('change', function () {
        this.value = this.checked ? 1 : 0;
    }).change();

    $('#chkInvalidFLG').on('change', function () {
        this.value = this.checked ? 1 : 0;
    }).change();
    
    $('#btnUpdate').on('click', function () {
        let model = {
            TestFLG: $('#chkTestFLG').val(),
            InvalidFLG: $('#chkInvalidFLG').val(),
            SellerCD: common.getUrlParameter('SellerCD')
        };

        common.callAjaxWithLoading(_url.insert_M_Seller, model, this, function (result) {
            if (result && result.isOK) {
                
            } else if (result && !result.isOK) {
                const errors = result.data;
                for (key in errors) {
                    const target = document.getElementById(key);
                    $(target).showError(errors[key]);
                }
            }
        });
    });


    $('#btnSuccess,#btnCancel').on('click', function () {
         window.location.reload();
    });
}

function get_account_Data(model, $form) {
    common.callAjaxWithLoadingSync(_url.get_t_seller_Info, model, this, function (result) {
        if (result && result.isOK) {
            Bind_Info(result.data);
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    })
}

function Bind_Info(result) {
    var data = JSON.parse(result);
    if (data.length > 0) { 
        if (data[0]['InvalidFLG'] == '無効会員')
            $("#chkInvalidFLG").prop("checked", true);

        if (data[0]['TestFLG'] == '1')
            $("#chkTestFLG").prop("checked", true);
    }
}






