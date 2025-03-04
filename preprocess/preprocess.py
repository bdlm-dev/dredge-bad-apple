import cv2
import json


"""
Process 'bad-apple.mp4' into json array of frames at target resolution, against a target fps.

every nth frame is processed,  n = video_fps / target_fps
"""

video_path = "bad-apple.mp4"
output_path = "frame-data.json"

resolution = (64, 64)
fps = 15


def preprocess(video_path, output_path, resolution, target_fps, save=None):

    capture = cv2.VideoCapture(video_path)
    fps = capture.get(cv2.CAP_PROP_FPS)
    frame_length = fps // target_fps

    print(f"Processing {capture.get(cv2.CAP_PROP_FRAME_COUNT) // 1} frames of {video_path} to {output_path}")
    print(f"Target resolution: {resolution}; Target fps: {target_fps}")

    frames = []
    frame_count = 0

    while capture.isOpened():

        ret, frame = capture.read()

        if not ret:
            break

        if frame_count % frame_length == 0:

            # 'needs to be greyscale', so just to make sure
            grey = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

            # extract frame as on/off
            resized = cv2.resize(grey, resolution, interpolation=cv2.INTER_AREA)

            # threshold for pure black/white
            _, binary = cv2.threshold(resized, 128, 1, cv2.THRESH_BINARY_INV)

            if save:
                binary_bgr = cv2.cvtColor(binary * 255, cv2.COLOR_GRAY2BGR)
                save.write(binary_bgr)

            frames.append(binary.tolist())

        frame_count += 1

    capture.release()
    if save:
        save.release()

    with open(output_path, "w+") as f:
        json.dump(frames, f)

    print(f"Saved {len(frames)} frames to {output_path}")


if __name__ == "__main__":

    save_as_video = False

    if save_as_video:
        fourcc = cv2.VideoWriter.fourcc(*'FMP4')
        out = cv2.VideoWriter('sample_output.avi', fourcc, 20.0, resolution)

        preprocess(video_path, output_path, resolution, fps, save=out)

        cap = cv2.VideoCapture('sample_output.avi')

        while cap.isOpened():
            ret, frame = cap.read()

            # if frame is read correctly ret is True
            if not ret:
                print("Can't receive frame (stream end?). Exiting ...")
                break

            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

            cv2.imshow('frame', gray)
            if cv2.waitKey(1) == ord('q'):
                break

        cap.release()
        cv2.destroyAllWindows()

    else:
        preprocess(video_path, output_path, resolution, fps)
