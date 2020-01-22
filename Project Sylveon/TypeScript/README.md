How this folder works
====
1. Create a typescript (.ts) file in *this* folder.
2. Program.
3. Save.
4. Run the .sh file (you'll need git bash!)
5. It gets pushed to ~/wwwroot/ts
6. Add a script reference in the relevant HTML page (e.g. `<script src="~/ts/example.js" type="module"></script>`)
7. ???
8. Profit

.js Files in the ~/wwwroot/tst folder are ignored by git by default.

Make sure that when you import a library, you add the `.js` extension to the end of it, or it will not work.