
function setValidation() {
    $('#MansionName')
        .addvalidation_errorElement("#errorMansionName")
        .addvalidation_maxlengthCheck(25)//E105
        .addvalidation_doublebyte();

    $('#StartAge')
        .addvalidation_errorElement("#errorAge")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104

    $('#EndAge')
        .addvalidation_errorElement("#errorAge")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104

    $('#StartDistance')
        .addvalidation_errorElement("#errorDistance")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104

    $('#EndDistance')
        .addvalidation_errorElement("#errorDistance")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104

    $('#StartRooms')
        .addvalidation_errorElement("#errorRooms")
        .addvalidation_maxlengthCheck(3)//E105
        .addvalidation_singlebyte_number()//E104

    $('#EndRooms')
        .addvalidation_errorElement("#errorRooms")
        .addvalidation_maxlengthCheck(3)//E105
        .addvalidation_singlebyte_number()//E104
}


