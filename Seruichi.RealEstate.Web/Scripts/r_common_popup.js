function Bind_site_error_modal($form, $error) {
    let modal = "";
    modal= '<div class="modal-dialog modal-dialog-centered">\
                <div class="modal-content">\
                    <div class="modal-header">\
                        <h5 class="modal-title d-none" id="staticBackdropLabel">' + $error + '</h5>\
                        <button type="button" class="btn-close d-none" data-bs-dismiss="modal" aria-label="Close"></button>\
                    </div>\
                     <div class="modal-body">\
                        該当データがありません。もう一度、条件を変更の上ダウンロードボタンを押してください。\
                     </div>\
                     <div class="modal-footer">\
                        <button type="button" class="btn btn-secondary" id="r_contact_close_site_error_modal" data-bs-dismiss="modal">閉じる</button>\
                     </div>\
                 </div>\
            </div>'
    $('#' + $form).append(modal);
    $('#' + $form).modal('show');
}