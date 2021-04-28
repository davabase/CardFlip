import bpy
from bpy_extras.object_utils import world_to_camera_view
from mathutils import Vector

path = '<path-to-monogame-projects-content-folder>'
filename = 'animation.txt'

scene = bpy.context.scene

object = bpy.data.objects['Plane']
plane = bpy.data.meshes['Plane']
polygons = plane.polygons

cam = bpy.data.objects['Camera']

# https://blender.stackexchange.com/questions/882/how-to-find-image-coordinates-of-the-rendered-vertex
render_scale = scene.render.resolution_percentage / 100
render_size = (
    int(scene.render.resolution_x * render_scale),
    int(scene.render.resolution_y * render_scale)
)

# https://blenderartists.org/t/how-to-get-vertices-coordinates-from-animated-mesh/565581/4
# Loop through all frames:
origin_coord = world_to_camera_view(scene, cam, plane.vertices[2].co)
origin = ((round(origin_coord.x * render_size[0]), round(origin_coord.y * render_size[1])))
origin_x = None
origin_y = None

total_output = ''
for frame in range(scene.frame_start, scene.frame_end):
    # Set the frame.
    scene.frame_set(frame)

    # Create a temporary mesh.
    mesh = object.to_mesh()
    # Apply local roation, position, and scale.
    mesh.transform(object.matrix_world)

    vertices = mesh.vertices
    verts = [vert.co for vert in vertices]
    coords_2d = [world_to_camera_view(scene, cam, coord) for coord in verts]
    pixel_coords = [(round(coord.x * render_size[0]), round(coord.y * render_size[1])) for coord in coords_2d]
    # We consider the first point (top left corner) of the first frame to be the origin.
    if not origin_x:
        origin_x = pixel_coords[2][0]
        origin_y = pixel_coords[2][1]

    # Clear the temporary mesh.
    object.to_mesh_clear()
    
    output = ''
    for x, y in pixel_coords:
        x -= origin_x
        y = origin_y - y
        output += f'{x} {y} '

    # The order is bottom left, bottom right, top left, top right
    total_output += output + '\n'

with open(path + filename, 'w+') as f:
    f.write(total_output)

print(total_output)
