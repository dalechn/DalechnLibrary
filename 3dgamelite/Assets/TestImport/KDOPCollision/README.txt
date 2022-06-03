K-DOP COLLISION HULL GENERATOR
¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
Three Eyed Games


DESCRIPTION
¯¯¯¯¯¯¯¯¯¯¯
Generate collision hulls for your meshes with only a few clicks!

A K-DOP is a k-dimensional discrete oriented polytope, i.e. a set of inter-
secting plane pairs that together form a simple hull for your object.

* K-DOPs: popular from other big Game Engines
* Automatic collision hulls without manual effort
* High-performance collision with a good approximation of the object's shape
* Works with simple meshes, compound objects and prefabs

A box can be thought of as a 6DOP, since it has 6 planes (or 3 plane 
pairs) that bound an object. Next in line is the 10DOP, which is a plane 
with the edges beveled in one direction (X, Y or Z). If all edges are 
beveled, you end up with an 18DOP. If the corners of the box are beveled 
too, you get a 26DOP. Just try them out and see which fits your object!


USAGE
¯¯¯¯¯
To use the script, first choose a folder where your collision meshes will
be stored. Next, select one or multiple GameObjects and click any of the
menu entries under 'GameObject/K-DOP Collision'. K-DOP collision hulls
will be generated for all your GameObjects and saved next to your mesh
assets for reusability.


CHANGELOG
¯¯¯¯¯¯¯¯¯
1.0 - Sep 20, 2016:
* First release

1.1 - Nov 16, 2016:
* Huge editor performance improvement: generating K-DOPs for large meshes now takes seconds instead of hours