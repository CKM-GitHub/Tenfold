const _url = {};
$(function () {
    _url.get_t_seller_Info = common.appPath + '/t_seller_memo/get_t_seller_Info';
    _url.get_t_seller_memo_DisplayData = common.appPath + '/t_seller_memo/get_t_seller_memo_DisplayData';
    _url.Insert_L_Log = common.appPath + '/t_seller_memo/Insert_L_Log';
    _url.Delete_Cmd = common.appPath + '/t_seller_memo/Delete_Cmd';
    addEvents();
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_reale_purchase').addClass('font-bold text-underline');
});

function addEvents() {
    $('#reale').addClass('d-none');
    $('#submenu_reale').addClass('d-none');
    Bind_Company_Data(this);         //Bind Company Info Data to the title part of the page

    var model = {
        SellerCD: common.getUrlParameter('SellerCD')
    }
    get_memo_Data(model, this);
}

function get_memo_Data(model, $form) {
    common.callAjaxWithLoadingSync(_url.get_t_seller_memo_DisplayData, model, this, function (result) {
        if (result && result.isOK) {
            Bind_memo_Data(result.data);
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

function Bind_memo_Data(result) {
    var data = JSON.parse(result);
    var p_html = '', c_html = '';
    var loginID = $('#loginID').text();
    for (var i = 0; i < data.length; i++) {
        if (data[i]['ParentChildKBN'] == '1') {
            p_html =
                '<div id="p_' + data[i]['SellerMemoSEQ'] + '" class="p-md-5 container">\
                <div class="row pb-3 col-md-12">\
                <div class="d-md-flex">\
                <strong>' + data[i]['TenStaffName'] + '</strong>\
                <p class="ps-md-5 text-secondary text-start">' + data[i]['UpdateDateTime'] + '</p>\
                </div>\
                <div class="clearfix"></div>\
                <p ondblclick="Edit_Cmd(\'' + data[i]['SellerMemoSEQ'] + '\')">' + data[i]['MemoText'] + '</p>\
                <p>\
                <button type="button" class="float-right btn btn-outline-primary ml-2" onclick="Reply_Cmd(\'' + data[i]['SellerMemoSEQ'] + '\')">\
                    <i class="fa fa-reply"></i> コメント </button>\
                <button type="button" id="btnDelete" class="float-right btn text-white btn-danger" onclick="Delete_Cmd(\'' + data[i]['SellerMemoSEQ'] + '\')">\
                    <i class="fa fa-times"></i> 削除 </button>\
                </p>\
                </div>\
                </div>';

            $('#display_area').append(p_html);
        }
        else if (data[i]['ParentChildKBN'] == '2') {
            c_html =
                '<div class="card-body">\
                <div class="row col-md-12">\
                <div class="d-md-flex">\
                <strong>' + data[i]['TenStaffName'] + '</strong>\
                <p class="ps-md-5 text-secondary text-start">' + data[i]['UpdateDateTime'] + '</p>\
                </div>\
                <p ondblclick="Edit_Cmd(\'' + data[i]['SellerMemoSEQ'] + '\')">' + data[i]['MemoText'] + '</p>\
                <p>\
                <button type="button" id="btnDelete" class="float-right btn text-white btn-danger" onclick="Delete_Cmd(\'' + data[i]['SellerMemoSEQ'] + '\')">\
                    <i class="fa fa-times"></i> 削除 </button>\
                </p>\
                </div>\
                </div>'

            if ($('#c_' + data[i]['ParentSEQ']).length < 1)
                $('#p_' + data[i]['ParentSEQ']).append('<div id="c_' + data[i]['ParentSEQ'] + '" class="card card-inner bg-gray-50 ms-md-24"></div>');  /*for the first child of the parent*/
            else
                $('#c_' + data[i]['ParentSEQ']).append('<hr class="navbar-divider my-5 opacity-20">');  /*to separate child div with space*/

            $('#c_' + data[i]['ParentSEQ']).append(c_html);
        }
    }
    if (loginID !== 'admin')
        $('#btnDelete').addClass('d-none');
}

function Edit_Cmd(SellerMemoSEQ) {
    var model = {
        SellerCD: common.getUrlParameter('SellerCD'),
        SellerMemoSEQ: SellerMemoSEQ
    }
    $('#message-com').modal('show');
}

function Reply_Cmd(SellerMemoSEQ) {
    var model = {
        SellerCD: common.getUrlParameter('SellerCD'),
        ParentChildKBN: '2',
        ParentSEQ: SellerMemoSEQ
    }
    $('#message-com').modal('show');
}

function Delete_Cmd(SellerMemoSEQ) {
    var model = {
        SellerMemoSEQ: SellerMemoSEQ,
    }
    $('#d_cmd').on('click', function () {
        Delete(model);
    });
    $('#message-del').modal('show');
}

function Delete(model) {
    $('#message-del').modal('hide');
    common.callAjaxWithLoadingSync(_url.Delete_Cmd, model, this, function (result) {
        if (result && result.isOK) {
            $('#message-delok').modal('show');
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                this.getInvalidItems().get(0).focus();
            }
        }
    })
}