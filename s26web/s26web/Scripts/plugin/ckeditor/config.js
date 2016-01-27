/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
    config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_P;
    config.htmlEncodeOutput = true;
    config.toolbar_Full = [['Source', '-', 'Preview', '-', 'Templates'],
                        ['Undo', 'Redo', '-', 'SelectAll', 'RemoveFormat'],
                        ['Styles', 'Format', 'Font', 'FontSize'],
                        ['TextColor', 'BGColor'],
                        ['Maximize', 'ShowBlocks', '-', 'About'], '/',
                        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
                        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv'],
                        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
                        ['Link', 'Unlink', 'Anchor'],
                        ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'],
                        ['Code']];

    //var temp = location.pathname.substring("shb").split('/')[2];
    /*if (location.pathname.match(/Blog/) != null)
    {
        temp = location.pathname.substring(location.pathname.lastIndexOf("/") + 1);
    }*/
    
    config.filebrowserImageUploadUrl = RootPath + '/shb/Upload/' + temp_dir;
    //config.skin = "v2";
    /*config.toolbar_Full = [['Source', '-', 'Save', 'NewPage', 'Preview', '-', 'Templates'],
                        ['Undo', 'Redo', '-', 'SelectAll', 'RemoveFormat'],
                        ['Styles', 'Format', 'Font', 'FontSize'],
                        ['TextColor', 'BGColor'],
                        ['Maximize', 'ShowBlocks', '-', 'About'], '/',
                        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
                        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv'],
                        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
                        ['Link', 'Unlink', 'Anchor'],
                        ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'],
                        ['Code']];*/
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
};
