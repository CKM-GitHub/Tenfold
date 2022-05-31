//Start script code of t_reale_Info.cshtml -> Row No. 2 ~ 52
function Bind_Company_Data(model, $form) {
    common.callAjaxWithLoading(_url.get_t_reale_purchase_CompanyInfo, model, this, function (result) {
        if (result && result.isOK) {
            Bind_CompanyInfo(result.data);
            common.callAjaxWithLoading(_url.get_t_reale_purchase_CompanyCountingInfo, model, this, function (result) {
                if (result && result.isOK) {
                    Bind_CountingInfo(result.data);
                }
            })
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

function Bind_CompanyInfo(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        $('#InvalidFLG').text(data[0]['InvalidFLG']);
        $('#REKana').text(data[0]['REKana']);
        $('#REName').text(data[0]['REName']);
        $('#RealECD').text(data[0]['RealECD']);
        $('#Address').text(data[0]['Address']);
        $('#HousePhone').text(data[0]['HousePhone']);
        $('#Fax').text(data[0]['Fax']);
        $('#MailAddress').text(data[0]['MailAddress']);
        $('#PICName').text(data[0]['PICName']);
    }
}

function Bind_CountingInfo(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        $('#Constant').text(data[0]['査定数']);
        $('#Top5').text(data[0]['Top5']);
        $('#Top1').text(data[0]['Top1']);
        $('#Customers').text(data[0]['送客数']);
        $('#Deals').text(data[0]['商談数']);
        $('#Contracts').text(data[0]['成約数']);
        $('#Seller_Declines').text(data[0]['売主辞退数']);
        $('#Buyer_Declines').text(data[0]['買主辞退数']);
    }
}
//End script code of t_reale_Info.cshtml