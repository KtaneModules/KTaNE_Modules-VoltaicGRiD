from os import walk

filenames = next(
    walk("C:/Users/Dustin/source/repos/KtaneModkit2/TemplateZIP/HTML/img/Jurassic/"),
    (None, None, []),
)[2]

row = 0


def run():
    row = 0
    for x in range(0, len(filenames)):
        if row == 0:
            print("<tr>")
            row += 1
        elif row == 4:
            print("</tr>")
            print("<tr>")
            row = 1
        else:
            row += 1

        print("\t<td>")
        print(f'\t\t<div><img src="img/Jurassic/{filenames[x]}"></div>')
        print(
            f'\t\t<div class="caption">{filenames[x].split(".")[0].capitalize()}</div>'
        )
        print("\t</td>")


if __name__ == "__main__":
    run()
