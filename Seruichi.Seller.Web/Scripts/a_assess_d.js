const _url = {};
let _valFlg = false;
let _model = {
        AssID:null,
        AssSEQ: null,
        AssReqID: null,
        ProgressKBN: '1',
        RealECD: null,
        SellerCD: null,
        SellerMansionID: null,
        AssessAmount: null,
        IpAddress: null,
        SellerName: null,
        AssessType1: null,
        AssessType2: null,
        ConditionSEQ: null
}
$(function () { 
    _url.GetD_Mansion_Info = common.appPath + '/a_assess_d/GetD_Mansion_Info';
    _url.GetD_Spec_Info = common.appPath + '/a_assess_d/GetD_Spec_Info';
    _url.GetD_MansionRank_Info = common.appPath + '/a_assess_d/GetD_MansionRank_Info';
    _url.GetD_AreaRank_Info = common.appPath + '/a_assess_d/GetD_AreaRank_Info'; 
    _url.InsertL_Log = common.appPath + '/a_assess_d/InsertGetD_AssReqProgress_L_Log';
    _url.InsertAssess_d = common.appPath + '/a_assess_d/InsertAssess_d';
    SetMansionInfo(this);
    SetSpecInfo(this);
    SetMansionRank(this);
    SetAreaRank(this);
});

function SetModal(img, region, staff, rate, amount,realecd,assid,assseq,assreqid,asstype1,asstype2,conditionseq) {
    $('#realecd').val(realecd);
    var attach = '<tr>\
        <td>\
            <img  src="data:image/gif;base64,'+ img + '"\
                width="50" height="50"\
                class="rounded-circle my-n1"\
                alt="Avatar">\
                                        </td>\
            <td>'+ region + '<br>' + staff +'</td>\
                <td class="text-nowrap align-middle text-end flg-ratepop">\
                    '+rate+'%\
                                        </td>\
                <td class="text-nowrap align-middle text-end">\
                    <span class="text-danger">'+ amount +'</span>円\
                                        </td>\
                                    </tr>';
    $('#popModal').html('');
    $('#popModal').append(attach);
    if (_valFlg)
        $('.flg-ratepop').addClass('hideAssess');

    //debugger
    _model.AssID = assid;
    _model.AssSEQ = assseq;
    _model.AssReqID = assreqid;
    _model.AssessType1 = asstype1;
    _model.AssessType2 = asstype2;
    _model.ConditionSEQ = conditionseq;
    _model.RealECD = realecd;
    _model.AssessAmount = amount;
}
function SetAreaRank($form) {

    let model = {
        SellerCD: null
    }
    return common.callAjaxWithLoading(_url.GetD_AreaRank_Info, model, this, function (result) {
        if (result && result.isOK) {
            var arr = ($.parseJSON(result.data));
            var attach = '';
            //var hidevl = '<th class="text-end text-nowrap">成約率</th>';

            if (arr[0].Flg != '') {
                $('.flg-rate').addClass('hideAssess');
                _valFlg = true;
            }
            //    $('#Arearateval').append(hidevl);
            //alert(result.data)
            for (var i = 0; i < arr.length; i++) {
                var f = (arr[i]);
                attach += '<tr>\
                    <td>\
                        <img  src="data:image/gif;base64,'+ f.REFaceImage + '"\
                            width="50" height="50" class="rounded-circle my-n1"\
                            alt="Avatar">\
                                    </td>\
                        <td>'+ f.Prefname + '<br>' + f.ReStaffName + '</td>\
                            <td class="text-nowrap align-middle text-end " style="display:'+f.Flg+';">'+ f.ContRateMansion + '%</td>\
                            <td class="text-nowrap align-middle text-end">\
                                <span class="text-danger">'+ f.AssessAmount + '</span>円\
                                    </td>\
                            <td class="align-middle">\
                                <a href="#" class="btn btn-primary text-nowrap"\
                                    data-bs-target="#modal-1" data-bs-toggle="modal"\
     role="button" aria-pressed="true" onclick="SetModal(\''+ f.REFaceImage + '\',\'' + f.Prefname + '\',\'' + f.ReStaffName + '\',\'' + f.ContRateMansion + '\',\'' +
                    f.AssessAmount + '\',\'' + f.RealECD + '\',\'' + f.AssID + '\',\'' + f.AssSEQ + '\',\'' + f.AssReqID + '\',\'' + f.AssessType1 + '\',\'' + f.AssessType2 + '\',\'' + f.ConditionSEQ + '\')">買取依頼</a>\
 </td>\
                                </tr>';
            }
            $('#bdyArea').append(attach);

        }
    });

}
function SetMansionRank($form) {

    let model = {
        SellerCD: null
    }
    return  common.callAjaxWithLoading(_url.GetD_MansionRank_Info, model, this, function (result) {
        if (result && result.isOK) {
            var arr = ($.parseJSON(result.data));
            var attach = '';
            //var hidevl = '<th class="text-end text-nowrap">成約率</th>';
            //if (arr[0].Flg = '1')
            //    $('.flg-rate').addClass('hideAssess');
            //$('#Mansionrateval').append(hidevl);
            //alert(result.data)
            for (var i = 0; i < arr.length; i++) {
                var f = (arr[i]);
                attach += '<tr>\
                    <td>\
                        <img  src="data:image/gif;base64,'+f.REFaceImage+'"\
                            width="50" height="50" class="rounded-circle my-n1"\
                            alt="Avatar">\
                                    </td>\
                        <td>'+ f.Prefname + '<br>' + f.ReStaffName+'</td>\
                            <td class="text-nowrap align-middle text-end flg-rate" style="display:'+ f.Flg +';">'+f.ContRateMansion+'%</td>\
                            <td class="text-nowrap align-middle text-end">\
                                <span class="text-danger">'+f.AssessAmount+'</span>円\
                                    </td>\
                         <td class="align-middle">\
                                <a href="#" class="btn btn-primary text-nowrap"\
                                    data-bs-target="#modal-1" data-bs-toggle="modal"\
     role="button" aria-pressed="true" onclick="SetModal(\''+ f.REFaceImage + '\',\'' + f.Prefname + '\',\'' + f.ReStaffName + '\',\'' + f.ContRateMansion + '\',\'' +
                    f.AssessAmount + '\',\'' + f.RealECD + '\',\'' + f.AssID + '\',\'' + f.AssSEQ + '\',\'' + f.AssReqID + '\',\'' + f.AssessType1 + '\',\'' + f.AssessType2 + '\',\'' + f.ConditionSEQ + '\')">買取依頼</a>\
 </td>\
                                </tr>';
            }
            $('#bdyMansion').append(attach);

        }
    }, function (completedata) {
           // $('.flg-rate').addClass('hideAssess');
    });

} 
function SetSpecInfo($form) {

    let model = {
        SellerCD: null
    }
    return common.callAjaxWithLoading(_url.GetD_Spec_Info, model, this, function (result) {
        if (result && result.isOK) {
            var arrJson = ($.parseJSON(result.data));
            if (arrJson.length) {
                var area = (arrJson[0]);
                $('#specBuildingStructure').text(area.BuildingStructure);
                $('#specYearContruction').text(area.ConstYYYYMM);
                $('#specRoomNo').text(area.RoomNumber);
                $('#specArea').text(area.RoomArea);
                $('#specBalconArea').text(area.BalconyArea);
                $('#specSunlighting').text(area.Direction);
                $('#specNumberofRoom').text(area.NumberofRoom);
                $('#specBathRoomToilet').text(area.BathRoomandToilet);
                $('#specLandLight').text(area.LandRight);
                $('#specPresentState').text(area.PresentState);
                $('#specManagementSystem').text(area.ManagementSystem);
                $('#specDesiredTimeSale').text(area.DesiredTimeSale);
                $('#specRentFee').text(area.RentFee);
                $('#specManagementFee').text(area.ManagementFee); 
                $('#specRepairFee').text(area.RepairFee);
                $('#specTax').text(area.PropertyFee);
                $('#specOtherFee').text(area.ExtraFee);
            } 
             
        }
    });

}
function SetMansionInfo($form) {
  
    let model = {
        SellerCD: null
    }
    return common.callAjaxWithLoading(_url.GetD_Mansion_Info, model, this, function (result) {
        if (result && result.isOK) {
            var arrJson = ($.parseJSON(result.data));
            if (arrJson.length) {
                var header = (arrJson[0]);
                $('#headerMansionName').text(header.MansionName);
                $('#headerAddress').text(header.Address);
                $('#popMansion').text(header.MansionName);
                $('#popAddress').text(header.Address);
                _model.SellerMansionID = header.SellerMansionID;
                _model.SellerName = header.SellerName;
            }
          
            var attach = '';
            for (var i = 0; i < arrJson.length; i++) {
                var f = (arrJson[i]); 
                attach += '<div class="card p-1 col-12 col-md-6 shadow-sm">\
                    <div class="card-body p-0 row">\
                        <div class="col-12">\
                            <h5> '+f.LineName +' </h5>\
                        </div>\
                        <div class="col-12">\
                            <p class="text-dark">'+f.StationName+'</p>\
                        </div>\
                        <div class="col-12 text-end">\
                            <p class="text-dark">'+f.Distance+'</p>\
                        </div>\
                    </div>\
                </div>'; 
            }
            $('#ekikoutsu').append(attach);

             
           // Bind_tbody(result.data);
        } 
    });

}
function l_logfunction(ecd) {
    let model = {
        SellerCD: null,
        Link: ecd
    }
    common.callAjax(_url.InsertL_Log, model,
        function (result) {
            if (result && result.isOK) {
              //  window.location.href = window.location.protocol + "//" + window.location.host + "/" + link + "?AssReqID=" + $('#assessreqid').text();
            }
        });
}
function InsertA_Assess() {
    common.callAjax(_url.InsertAssess_d, _model,
        function (result) {
            if (result && result.isOK) {
                //  window.location.href = window.location.protocol + "//" + window.location.host + "/" + link + "?AssReqID=" + $('#assessreqid').text();
            }
        }); 
}
$('#applytoSell').click(function () {
    InsertA_Assess();
}); 
 