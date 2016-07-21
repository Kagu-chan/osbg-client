/**
 * Declares the kind of template which is required
 */
enum TemplateType {
    Component,
    Core,
    Share
};

/**
 * Module to get path's for specific templates
 */
module TemplatePath {
    var directoryNames: string[] = [
        "components",
        "core",
        "shared"
    ];

    /**
     * Gets the path to a template file
     * 
     * @param templateType
     * @param feature
     */
    export function get(templateType: TemplateType, feature: string):string {
        var subDir: string = directoryNames[templateType];
        var pathsArr: string[] = [
            $templatesPath,
            subDir,
            feature,
            "view.html"
        ];
        return pathsArr.join($directorySeperator);
    }
}